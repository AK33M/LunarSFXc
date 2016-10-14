using LunarSFXc.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    public class AdminController : Controller
    {
        private IBlogRepository _repo;

        public AdminController(IBlogRepository repo)
        {
            _repo= repo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
