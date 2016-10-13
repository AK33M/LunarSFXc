using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class CommentForm : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new CommentViewModel();

            return View("CommentForm", model);
        }
    }
}
