using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var events = await _repo.GetTimelineEvents("aboutMe");
                var timelineViewModels = new List<TimelineEventViewModel>();

                foreach (var ev in events)
                {
                    var foo = Mapper.Map<TimelineEventViewModel>(ev);
                    foo.Image.ImageUri = _cloudService.GetImageUri(foo.Image.ContainerName, foo.Image.FileName);

                    timelineViewModels.Add(foo);
                }

                return Content(JsonConvert.SerializeObject(new
                {
                    records = timelineViewModels.Count,
                    rows = timelineViewModels
                }), "application/Json");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Events: {ex}");
                return BadRequest("Error occurred");
            }
        }
    }
}
