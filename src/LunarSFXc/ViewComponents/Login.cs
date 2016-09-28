using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class Login : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new LoginViewModel();

            return View("Login", model);
        }
    }
}
