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

        public IActionResult Category(string category, int p = 1)
        {
            //var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            //if (viewModel.Category == null)
            //    throw new HttpException(404, "Category not found");

            //ViewBag.Title = string.Format(@"Latest posts on category ""{0}""",
            //                    viewModel.Category.Name);
            return View();
        }

        public IActionResult Tag(string tag, int p = 1)
        {
            //var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            //if (viewModel.Tag == null)
            //    throw new HttpException(404, "Tag not found");

            //ViewBag.Title = string.Format(@"Latest posts tagged on ""{0}""",
            //    viewModel.Tag.Name);
            //return View("List", viewModel);
            return View();
        }
    }
}
