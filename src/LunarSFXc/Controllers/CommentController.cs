using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
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
                if (!ModelState.IsValid)
                    throw new Exception("Invalid ModelState");

                var comment = Mapper.Map<Comment>(commentModel);
                comment.ParentPost = _repo.Post(commentModel.Year, commentModel.Month, commentModel.PostTitle);
                comment.Owner = _userManager.FindByNameAsync(commentModel.User).Result;

                _repo.AddOrUpdateComment(comment);

                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(new { Message = "Comment saved" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }
    }
}
