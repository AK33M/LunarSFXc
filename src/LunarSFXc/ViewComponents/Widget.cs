using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class Widget : ViewComponent
    {
        private IBlogRepository _repo;

        public Widget(IBlogRepository repo)
        {
            _repo = repo;
        }
        public IViewComponentResult Invoke()
        {
            var model = new WidgetViewModel(_repo);

            return View("Default", model);
        }
    }
}
