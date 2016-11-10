using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers
{
    public class CommentController : Controller
    {
        private IBlogRepository _repo;
        private UserManager<LunarUser> _userManager;

        public CommentController(IBlogRepository repo, UserManager<LunarUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Save(CommentViewModel commentModel)
        {
            try
            {
                var comment = Mapper.Map<Comment>(commentModel);
                comment.ParentPost = _repo.Post(commentModel.Year, commentModel.Month, commentModel.PostTitle);
                comment.Owner =  _userManager.FindByNameAsync(commentModel.User).Result;

                _repo.AddOrUpdateComment(comment);

                return RedirectToActionPermanent("Post", "Blog", new
                {
                    year = comment.ParentPost.PostedOn.Year,
                    month = comment.ParentPost.PostedOn.Month,
                    title = comment.ParentPost.UrlSlug
                });
                //return ViewComponent("Comments", new { comments = Mapper.Map<ICollection<CommentViewModel>>(comment.ParentPost.Comments) });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
