using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
