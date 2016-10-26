using LunarSFXc.Extensions;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/images")]
    public class ImagesController : Controller
    {
        private IOptions<ImageServiceOptions> _imageServiceOptions;
        private IBlogRepository _repo;
        private ICloudStorageService _cloudStorage;
        private ILogger<ImagesController> _logger;

        public ImagesController(IBlogRepository repo, IOptions<ImageServiceOptions> imageServiceOptions, ICloudStorageService cloudStorage, ILogger<ImagesController> logger)
        {
            _repo = repo;
            _imageServiceOptions = imageServiceOptions;
            _cloudStorage = cloudStorage;
            _logger = logger;
        }

        [Route("upload")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(Image file, string containerName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // http://www.mikesdotnetting.com/article/288/asp-net-5-uploading-files-with-asp-net-mvc-6
                    // http://dotnetthoughts.net/file-upload-in-asp-net-5-and-mvc-6/
                    if (file.File.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.File.ContentDisposition).FileName.Trim('"');

                        // Extension method update RC2 has removed this 
                        //await file.File.SaveAsAsync(Path.Combine(_imageServiceOptions.Value.ServerUploadFolder, fileName));
                        var container = _cloudStorage.GetStorageContainer(containerName);
                        await file.File.SaveInAzureAsync(container, fileName);

                        var imageDesc = new ImageDescription
                        {
                            ContentType = file.File.ContentType,
                            FileName = file.File.FileName,
                            ContainerName = container.Name,
                            CreatedTimestamp = DateTime.UtcNow,
                            UpdatedTimestamp = DateTime.UtcNow,
                            Description = file.Id
                        };

                         var imageId = await _repo.AddOrUpdateFileDescriptions(imageDesc);

                        return Ok(new { Message = "Image uploaded", ImageId = imageId });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error loading Image", ex);
                    ModelState.AddModelError("", "Error loading Image");
                    return BadRequest(new { Message = "Error loading Image" });
                }
            }

            ModelState.AddModelError("", "Invalid Model Error");
            return BadRequest(new { Message = "Image could not be uploaded, please try again" });
        }



        [Route("download/{id}")]
        [HttpGet]
        public async Task<FileStreamResult> Download(int id)
        {
            var fileDescription = await _repo.GetFileDescription(id);

            var path = _imageServiceOptions.Value.ServerUploadFolder + "\\" + fileDescription.FileName;
            var stream = new FileStream(path, FileMode.Open);
            return File(stream, fileDescription.ContentType);
        }

        [Route("list")]
        [HttpGet]
        public async Task<ICollection<Uri>> ListAllImages(string containerName)
        {
            return await _cloudStorage.ListAllBlobs(containerName);
        }
    }
}
