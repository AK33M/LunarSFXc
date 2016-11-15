using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers.Api
{
    [Route("api/portfolio")]
    [Authorize]
    public class ProjectsController : Controller
    {
        private ICloudStorageService _cloudService;
        private ILogger<ProjectsController> _logger;
        private IBlogRepository _repo;

        public ProjectsController(IBlogRepository repo, ICloudStorageService cloudService, ILogger<ProjectsController> logger)
        {
            _cloudService = cloudService;
            _repo = repo;
            _logger = logger;
        }

        [Route("projects")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var model = new PortfolioViewModel(_cloudService, _repo);

                return Content(JsonConvert.SerializeObject(new
                {
                    records = model.Projects.Count,
                    rows = model.Projects
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Projects: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [Route("project/{id}")]
        [HttpGet]
        public IActionResult Get(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var model = new PortfolioViewModel(_cloudService, _repo, Id);
                    var results = (List<ProjectViewModel>)Mapper.Map<ICollection<ProjectViewModel>>(model.Projects);

                    return Json(results.ToArray()[0]);
                }
                else
                {
                    return Json(new ProjectViewModel());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get portfolio project", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [Route("save")]
        [HttpPost]
        public IActionResult Post([FromBody]ProjectViewModel portfolioProject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newProject = Mapper.Map<Project>(portfolioProject);

                    //Save to the database
                    _logger.LogInformation("Attempting to save a new project");
                    _repo.AddOrUpdatePortfolioProject(newProject);

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<ProjectViewModel>(newProject));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Save new project", ex);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = ex.Message });
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed to Save new project", ModelState = ModelState });
        }


        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var result = await _repo.DeletePortfolioProject(Id);

                if (result > 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { Message = $"Project {Id} has been deleted!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete project {Id}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return Json(new { Message = $"Could not delete Project {Id}! Please try again later." });
        }
    }
}
