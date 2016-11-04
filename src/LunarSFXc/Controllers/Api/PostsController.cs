﻿using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private string _currentUserId;
        private ILogger<PostsController> _logger;
        private IBlogRepository _repo;
        private UserManager<LunarUser> _userManager;

        public PostsController(IBlogRepository repo, ILogger<PostsController> logger, IHttpContextAccessor httpContextAccessor, UserManager<LunarUser> userManager)
        {
            _repo = repo;
            _logger = logger;
            _userManager = userManager;
            _currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public IActionResult Get(PaginationOptions options)
        {
            try
            {
                var posts = _repo.Posts(options.pageNumber - 1, options.pageSize, options.columnName, options.sort == "asc");

                var postsViewModels = Mapper.Map<ICollection<PostViewModel>>(posts);
                var totalPosts = _repo.TotalPosts(false);

                //return Ok(postsViewModels);

                return Content(JsonConvert.SerializeObject(new
                {
                    page = options.pageNumber,
                    records = totalPosts,
                    rows = postsViewModels,
                    total = Math.Ceiling(Convert.ToDouble(totalPosts) / options.pageSize)
                }), "application/Json");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Posts: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [HttpGet]
        [Route("post/{year?}/{month?}/{title?}")]
        public IActionResult Get(int year, int month, string title)
        {
            try
            {
                var post = _repo.Post(year, month, title);

                if (post == null)
                    return Json(new PostViewModel());

                var model = Mapper.Map<PostViewModel>(post);

                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get post", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = Mapper.Map<ICollection<CategoryViewModel>>(_repo.Categories());
                return Json(new { Categories = categories });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Categories: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [HttpGet]
        [Route("tags")]
        public IActionResult GetTags()
        {
            try
            {
                var tags = Mapper.Map<ICollection<TagViewModel>>(_repo.Tags());
                return Json(new { Tags = tags });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Categories: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Post([FromBody]PostViewModel postModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.FindByIdAsync(_currentUserId);

                try
                {
                    var post = Mapper.Map<Post>(postModel);

                    //Save to the database
                    _logger.LogInformation("Attempting to save a new post");
                    _repo.AddOrUpdateBlogPost(post, currentUser);

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<PostViewModel>(post));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Save new post", ex);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = ex.Message });
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed to Save new post", ModelState = ModelState });
        }
    }
}
