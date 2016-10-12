using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class PhotoGallery : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("PhotoGallery");
        }
    }
}
