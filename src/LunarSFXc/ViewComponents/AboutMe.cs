using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class AboutMe : ViewComponent
    {
        private ICloudStorageService _cloudService;
        private IBlogRepository _repo;

        public AboutMe(IBlogRepository repo, ICloudStorageService cloudService)
        {
            _repo = repo;
            _cloudService = cloudService;
        }

        public IViewComponentResult Invoke()
        {
            var model = new AboutMeViewModel(_cloudService, _repo);

            return View("AboutMe", model);
        }
    }
}
