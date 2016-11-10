using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class Widget : ViewComponent
    {
        private ICloudStorageService _cloudService;
        private IBlogRepository _repo;

        public Widget(IBlogRepository repo, ICloudStorageService cloudService)
        {
            _repo = repo;
            _cloudService = cloudService;
        }
        public IViewComponentResult Invoke(bool sidebar = true)
        {
            if (!sidebar)
            {
                var model = new WidgetViewModel(_repo, _cloudService);

                return View("Widget", model);
            }
            else
            {
                var model = new WidgetViewModel(_repo, _cloudService);

                return View("SidebarWidget", model);
            }                
        }
    }
}
