using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LunarSFXc.ViewComponents
{
    public class Comments : ViewComponent
    {
        private IBlogRepository _repo;

        public Comments(IBlogRepository repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke(ICollection<Comment> comments = null)
        {
            var model = new CommentsViewModel();

            if (comments != null)
            {
                model.Comments = comments;
                foreach (var c in model.Comments)
                {
                    c.Replies = _repo.GetChildComments(c.Id);
                }
            }

            return View("Comments", model);
        }
    }
}
