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

        [Route("upload")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(ImageDescriptionShort file)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            var files = new ImageResult();

            if (ModelState.IsValid)
            {
                // http://www.mikesdotnetting.com/article/288/asp-net-5-uploading-files-with-asp-net-mvc-6
                // http://dotnetthoughts.net/file-upload-in-asp-net-5-and-mvc-6/
                foreach (var f in file.File)
                {
                    if (f.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(f.ContentDisposition).FileName.Trim('"');
                        contentTypes.Add(f.ContentType);
                        names.Add(fileName);

                        // Extension method update RC2 has removed this 
                        await f.SaveAsAsync(Path.Combine(_imageServiceOptions.Value.ServerUploadFolder, fileName));
                    }

                    files.Files.Add(new ImageDescription
                    {
                        ContentType = f.ContentType,
                        CreatedTimestamp = DateTime.Now,
                        UpdatedTimestamp = DateTime.Now,
                        Description = "",
                        FileName = f.FileName
                    });
                }
            }


            _repo.AddFileDescriptions(files);

            //return RedirectToAction("ViewAllFiles", "FileClient");
            return Ok();
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
    }
}
