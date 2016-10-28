using AutoMapper;
using LunarSFXc.Extensions;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/images")]
    public class ImagesController : Controller
    {
        private IOptions<ImageServiceOptions> _imageServiceOptions;
        private IBlogRepository _repo;
        private ICloudStorageService _cloudService;
        private ILogger<ImagesController> _logger;

        public ImagesController(IBlogRepository repo, IOptions<ImageServiceOptions> imageServiceOptions, ICloudStorageService cloudService, ILogger<ImagesController> logger)
        {
            _repo = repo;
            _imageServiceOptions = imageServiceOptions;
            _cloudService = cloudService;
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
                        var container = _cloudService.GetStorageContainer(containerName);
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

                        var image =  _repo.AddOrUpdateFileDescriptions(imageDesc);
                        var imageModel = Mapper.Map<ImageDescriptionViewModel>(image);

                        GetImageUri(imageModel);

                        return Ok(new { Message = "Image uploaded", Image = imageModel });
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

        //[Route("download/{id}")]
        //[HttpGet]
        //public async Task<FileStreamResult> Download(int id)
        //{
        //    var fileDescription = await _repo.GetFileDescription(id);

        //    var path = _imageServiceOptions.Value.ServerUploadFolder + "\\" + fileDescription.FileName;
        //    var stream = new FileStream(path, FileMode.Open);
        //    return File(stream, fileDescription.ContentType);
        //}

        [Route("list")]
        [HttpGet]
        public GalleryViewModel ListAllImages(string containerName)
        {
            return new GalleryViewModel(_cloudService, _repo, containerName);
        }

        ////TODO:What is this?
        //[Route("{Id}")]
        //[HttpGet]
        //public async Task<string> ImageUri(int Id)
        //{
        //    var desc = await _repo.GetFileDescription(Id);
        //    var imguri = _cloudService.GetImageUri(desc.ContainerName, desc.FileName);

        //    return imguri;
        //}

        private void GetImageUri(ImageDescriptionViewModel model)
        {
           model.ImageUri = _cloudService.GetImageUri(model.ContainerName, model.FileName);
        }

    }
}
