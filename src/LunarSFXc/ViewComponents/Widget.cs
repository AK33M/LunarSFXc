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
        public IViewComponentResult Invoke(bool sidebar = true)
        {
            if (!sidebar)
            {
                var model = new WidgetViewModel(_repo);

                return View("Widget", model);
            }
            else
            {
                var model = new WidgetViewModel(_repo);

                return View("SidebarWidget", model);
            }                
        }
    }
}
