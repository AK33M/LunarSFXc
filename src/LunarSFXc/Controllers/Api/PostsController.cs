using AutoMapper;
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
        public IActionResult Get(JqInViewModel jqParams)
        {
            try
            {
                var posts = _repo.Posts(jqParams.page - 1, jqParams.rows, jqParams.sidx, jqParams.sord == "asc");

                var postsViewModels = Mapper.Map<ICollection<PostViewModel>>(posts);

                //return Ok(postsViewModels);

                return Content(JsonConvert.SerializeObject(new
                {
                    page = jqParams.page,
                    records = postsViewModels.Count,
                    rows = postsViewModels,
                    total = 1//Math.Ceiling(Convert.ToDouble(posts.Count) / jqParams.rows)
                }), "application/Json");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Posts: {ex}");
                return BadRequest("Error occurred");
            }
        }
    }
}
