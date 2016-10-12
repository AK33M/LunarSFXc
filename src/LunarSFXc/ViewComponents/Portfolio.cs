using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class Portfolio : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("Portfolio");
        }
    }
}
