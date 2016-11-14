using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class Portfolio : ViewComponent
    {
        private ICloudStorageService _cloudService;
        private IBlogRepository _repo;

        public Portfolio(ICloudStorageService cloudService, IBlogRepository repo)
        {
            _cloudService = cloudService;
            _repo = repo;
        }

        public IViewComponentResult Invoke()
        {
            var model = new PortfolioViewModel(_cloudService, _repo);

            return View("Portfolio", model);
        }
    }
}
