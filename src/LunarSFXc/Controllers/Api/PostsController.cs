using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private ILogger _logger;
        private IBlogRepository _repo;

        public PostsController(IBlogRepository repo, ILogger logger)
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

                return Ok(postsViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Posts: {ex}");
                return BadRequest("Error occurred");
            }
        }
    }
}
