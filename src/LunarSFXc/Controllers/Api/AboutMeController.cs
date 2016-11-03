using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        [Route("events")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var model = new AboutMeViewModel(_cloudService, _repo);

                return Content(JsonConvert.SerializeObject(new
                {
                    records = model.Events.Count,
                    rows = model.Events
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Events: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [Route("event/{id}")]
        [HttpGet]
        public IActionResult Get(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var model = new AboutMeViewModel(_cloudService, _repo, Id);
                    List<TimelineEventViewModel> results = (List<TimelineEventViewModel>)Mapper.Map<ICollection<TimelineEventViewModel>>(model.Events);

                    return Json(results.ToArray()[0]);
                }
                else
                {
                    return Json(new TimelineEventViewModel());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get event", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [Route("save")]
        [HttpPost]
        public IActionResult Post([FromBody]TimelineEventViewModel timelineEvent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newEvent = Mapper.Map<TimelineEvent>(timelineEvent);

                    //Save to the database
                    _logger.LogInformation("Attempting to save a new event");
                    _repo.AddOrUpdateTimelineEvent(newEvent);

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<TimelineEventViewModel>(newEvent));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Save new event", ex);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = ex.Message });
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed to Save new event", ModelState = ModelState });
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var result = await _repo.DeleteTimelineEvent(Id);

                if (result > 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { Message = $"Event {Id} has been deleted!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete event {Id}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return Json(new { Message = $"Could not delete Event {Id}! Please try again later." });
        }
    }
}
