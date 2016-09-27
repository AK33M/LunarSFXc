using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult LogIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
