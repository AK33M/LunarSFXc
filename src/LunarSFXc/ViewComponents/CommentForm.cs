using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class CommentForm : ViewComponent
    {
        public IViewComponentResult Invoke(string user, PostViewModel parentPost)
        {
            var model = new CommentViewModel(user, 
                                                parentPost.Title, 
                                                parentPost.PostedOn.Year, 
                                                parentPost.PostedOn.Month);

            return View("CommentForm", model);
        }
    }
}
