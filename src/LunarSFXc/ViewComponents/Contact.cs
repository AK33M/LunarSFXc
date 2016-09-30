using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class Contact : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new ContactViewModel();

            return View("Contact", model);
        }
    }
}
