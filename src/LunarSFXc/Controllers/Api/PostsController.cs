using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private ILogger<PostsController> _logger;
        private IBlogRepository _repo;

        public PostsController(IBlogRepository repo, ILogger<PostsController> logger)
        {
            _repo = repo;
            _logger = logger;
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
    }
}
