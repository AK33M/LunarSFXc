using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LunarSFXc.Controllers
{
    public class CommentController : Controller
    {
        private IBlogRepository _repo;

        public CommentController(IBlogRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Save(CommentViewModel commentModel)
        {
            try
            {
                var comment = Mapper.Map<Comment>(commentModel);
                _repo.AddOrUpdateComment(comment);

                return ViewComponent("Comments");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
