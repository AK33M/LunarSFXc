using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Akeem Taiwo";
            return View();
        }
    }
}
