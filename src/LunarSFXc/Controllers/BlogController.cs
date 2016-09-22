using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LunarSFXc.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _repo;

        public BlogController(IBlogRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(_repo, p);
            //var viewModel = new ListViewModel
            //{
            //    Posts = new List<Post>(),
            //    TotalPosts = 1
            //};
            //viewModel.Posts.Add(new Post
            //{
            //    Id = 1,
            //    Title = "new title",
            //    PostedOn = DateTime.Today
            //});

            ViewBag.Title = "Latest Posts";

            return View("List", viewModel);
        }

        public IActionResult Post(int year, int month, string title)
        {
            //var post = _blogRepository.Post(year, month, title);

            //if (post == null)
            //    throw new HttpException(404, "Post not found");

            //if (post.Published == false && User.Identity.IsAuthenticated == false)
            //    throw new HttpException(401, "The post is not published");

            return View();
        }
    }
}
