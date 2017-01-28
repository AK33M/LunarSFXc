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
    [Route("api/user")]
    public class UserController : Controller
    {
        private ILogger<UserController> _logger;
        private IBlogRepository _repo;

        public UserController(IBlogRepository repo, ILogger<UserController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        [Route("users")]
        public IActionResult Get()
        {
            try
            {
                var users = _repo.Users();
                var model = Mapper.Map<ICollection<LunarUserViewModel>>(users);

                return Content(JsonConvert.SerializeObject(new
                {
                    records = model.Count,
                    rows = model
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all users: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [HttpGet]
        [Route("user/{userName}")]
        public IActionResult GetUser(string userName)
        {
            try
            {
                var model = Mapper.Map<LunarUserViewModel>(_repo.User(userName));

                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user: {ex}");
                return BadRequest("Error occurred");
            }
        }
    }
}
