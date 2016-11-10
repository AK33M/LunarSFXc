using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    public class BlogController : Controller
    {
        private IBlogRepository _repo;
        private ICloudStorageService _cloudService;

        public BlogController(IBlogRepository repo, ICloudStorageService cloudService)
        {
            _repo = repo;
            _cloudService = cloudService;
        }

        public IActionResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(_repo, _cloudService, p);

            ViewBag.Title = "Latest Posts";

            return View("List", viewModel);
        }

        public IActionResult Post(int year, int month, string title)
        {
            var post = _repo.Post(year, month, title);

            if (post == null)
                throw new Exception("Post not found");

            if (post.Published == false && User.Identity.IsAuthenticated == false)
                throw new Exception("The post is not published");

            var model = Mapper.Map<PostViewModel>(post);

            foreach (var item in model.Images)
            {
                item.ImageUri = _cloudService.GetImageUri(item.ContainerName, item.FileName);
            }

            return View(model);
        }

        public IActionResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(_repo, _cloudService, category, "Category", p);

            if (viewModel.Category == null)
                throw new Exception("Category not found");

            ViewBag.Title = $"Latest posts on category {viewModel.Category.Name}";

            return View("List", viewModel);
        }

        public IActionResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(_repo, _cloudService, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new Exception("Tag not found");

            ViewBag.Title = $"Latest posts tagged on {viewModel.Tag.Name}";

            return View("List", viewModel);
        }

        public IActionResult Search(string s, int p = 1)
        {
            ViewBag.Title = $"Lists of posts found for search text {s}";

            var viewModel = new ListViewModel(_repo, _cloudService, s, "Search", p);
            return View("List", viewModel);
        }
    }
}
