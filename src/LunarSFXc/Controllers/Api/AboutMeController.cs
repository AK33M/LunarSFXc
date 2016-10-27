﻿using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/aboutme")]
    public class AboutMeController : Controller
    {
        private ICloudStorageService _cloudService;
        private ILogger<AboutMeController> _logger;
        private IBlogRepository _repo;

        public AboutMeController(IBlogRepository repo, ICloudStorageService cloudService, ILogger<AboutMeController> logger)
        {
            _repo = repo;
            _logger = logger;
            _cloudService = cloudService;
        }

        [Route("list")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var model = new AboutMeViewModel(_cloudService, _repo);

                return Json(new
                {
                    records = model.Events.Count,
                    rows = model.Events
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Events: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [Route("post")]
        [HttpPost]
        public async Task<IActionResult> Post(TimelineEventViewModel timelineEvent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newEvent = Mapper.Map<TimelineEvent>(timelineEvent);

                    //Save to the database
                    _logger.LogInformation("Attempting to save a new event");
                    _repo.AddTimelineEvent(newEvent);

                    if (await _repo.SaveAllAsync())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TimelineEventViewModel>(newEvent));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Save new Event", ex);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = ex.Message });
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
