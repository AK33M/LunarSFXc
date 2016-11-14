using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LunarSFXc.ViewComponents
{
    public class Portfolio : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model =  new List<ProjectViewModel>();

            return View("Portfolio", model);
        }
    }
}
