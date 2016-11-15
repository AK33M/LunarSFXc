using LunarSFXc.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    [Authorize]
    public class AdminController : Controller
    {
        private IBlogRepository _repo;

        public AdminController(IBlogRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Admin";
            return View();
        }
    }
}
