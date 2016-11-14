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
    [Route("Comment")]
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
        [Route("Post")]
        public IActionResult Save(CommentViewModel commentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Invalid ModelState");

                var commentToSave = new Comment();

                if (commentModel.ParentCommentId != 0)
                {
                    commentToSave = _repo.Comment(commentModel.ParentCommentId);
                    var childComment = Mapper.Map<Comment>(commentModel);
                    childComment.CreatedDate = DateTime.Now;
                    childComment.Owner = _userManager.FindByNameAsync(commentModel.User).Result;

                    commentToSave.Replies.Add(childComment);
                }
                else if(commentModel.CommentId != 0)
                {
                    commentToSave = _repo.Comment(commentModel.CommentId);
                    commentToSave.Content = commentModel.Content;
                    commentToSave.ModifiedDate = DateTime.Now;
                }
                else
                {
                    Mapper.Map(commentModel, commentToSave);
                    commentToSave.ParentPost = _repo.Post(commentModel.Year, commentModel.Month, commentModel.PostTitle);
                    commentToSave.CreatedDate = DateTime.Now;
                    commentToSave.Owner = _userManager.FindByNameAsync(commentModel.User).Result;
                }

                _repo.AddOrUpdateComment(commentToSave);

                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(new { Message = "Comment saved" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [Route("GetComment")]
        public IActionResult GetComment(int id)
        {
            try
            {
                var comment = _repo.Comment(id);

                return Json(comment.Content);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [Route("DeleteComment")]
        [HttpPost]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                var comment = _repo.Comment(id);
                comment.Content = null;
                comment.ModifiedDate = DateTime.Now;
                _repo.DeleteComment(comment);

                return Json(new { Message = "Comment was deleted" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }
    }
}
