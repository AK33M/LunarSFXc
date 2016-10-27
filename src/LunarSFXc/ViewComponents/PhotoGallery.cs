using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class PhotoGallery : ViewComponent
    {
        private ICloudStorageService _cloudService;
        private IBlogRepository _repo;

        public PhotoGallery(ICloudStorageService cloudService, IBlogRepository repo)
        {
            _cloudService = cloudService;
            _repo = repo;
        }
        public IViewComponentResult Invoke(string containerName)
        {

            var model = new GalleryViewModel(_cloudService, _repo, containerName);

            return View("PhotoGallery", model);
        }
    }
}
