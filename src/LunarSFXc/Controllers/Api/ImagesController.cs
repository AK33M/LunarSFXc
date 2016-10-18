using LunarSFXc.Extensions;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ImagesController(IBlogRepository repo, IOptions<ImageServiceOptions> imageServiceOptions)
        {
            _repo = repo;
            _imageServiceOptions = imageServiceOptions;
        }

        [Route("images")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(ICollection<IFormFile> Files)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            var files = new ImageResult();

            if (ModelState.IsValid)
            {
                // http://www.mikesdotnetting.com/article/288/asp-net-5-uploading-files-with-asp-net-mvc-6
                // http://dotnetthoughts.net/file-upload-in-asp-net-5-and-mvc-6/
                foreach (var file in Files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        contentTypes.Add(file.ContentType);
                        names.Add(fileName);

                        // Extension method update RC2 has removed this 
                        await file.SaveAsAsync(Path.Combine(_imageServiceOptions.Value.ServerUploadFolder, fileName));
                    }

                    files.Files.Add(new ImageDescription
                    {
                        ContentType = file.ContentType,
                        CreatedTimestamp = DateTime.Now,
                        UpdatedTimestamp = DateTime.Now,
                        Description = "",
                        FileName = file.FileName
                    });
                }
            }


            _repo.AddFileDescriptions(files);

            //return RedirectToAction("ViewAllFiles", "FileClient");
            return Ok();
        }

        [Route("download/{id}")]
        [HttpGet]
        public FileStreamResult Download(int id)
        {
            var fileDescription = _repo.GetFileDescription(id);

            var path = _imageServiceOptions.Value.ServerUploadFolder + "\\" + fileDescription.FileName;
            var stream = new FileStream(path, FileMode.Open);
            return File(stream, fileDescription.ContentType);
        }
    }
}
