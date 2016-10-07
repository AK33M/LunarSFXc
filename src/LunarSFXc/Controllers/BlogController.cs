using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    public class BlogController : Controller
    {
        private IBlogRepository _repo;
        private IEmailService _mailService;
        private IConfigurationRoot _config;

        public BlogController(IBlogRepository repo, IEmailService mailService, IConfigurationRoot config)
        {
            _repo = repo;
            _mailService = mailService;
            _config = config;
        }

        public IActionResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(_repo, p);

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

            return View(post);
        }

        public IActionResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(_repo, category, "Category", p);

            if (viewModel.Category == null)
                throw new Exception("Category not found");

            ViewBag.Title = $"Latest posts on category {viewModel.Category.Name}";
                               
            return View("List", viewModel);
        }

        public IActionResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(_repo, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new Exception("Tag not found");

            ViewBag.Title = $"Latest posts tagged on {viewModel.Tag.Name}";

            return View("List", viewModel);
        }

        public IActionResult Search(string s, int p = 1)
        {
            ViewBag.Title = $"Lists of posts found for search text {s}";

            var viewModel = new ListViewModel(_repo, s, "Search", p);
            return View("List", viewModel);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _mailService.SendEmailAsync(model.Email, "Message from Website", model.Message);
                // (_config["mailSettings:recipientAddress"], model.Email, "Message from LunarSFX", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Success. Message Sent!";
            }
            else
            {
                ModelState.AddModelError("", "Something Happened!");
                return BadRequest("Could not send your message, try again later.");
            }            

            return Ok(ViewBag.UserMessage);
        }
    }
}
