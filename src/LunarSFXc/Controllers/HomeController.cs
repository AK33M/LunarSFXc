using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
