using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarSFXc.ViewComponents
{
    public class AboutMe : ViewComponent
    {
        private ICloudStorageService _cloudService;
        private IBlogRepository _repo;

        public AboutMe(IBlogRepository repo, ICloudStorageService cloudService)
        {
            _repo = repo;
            _cloudService = cloudService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _repo.GetTimelineEvents("aboutMe");
            var timelineViewModels = new List<TimelineEventViewModel>();

            foreach(var ev in events)
            {
                var foo = Mapper.Map<TimelineEventViewModel>(ev);
                foo.Image.ImageUri = _cloudService.GetImageUri(foo.Image.ContainerName, foo.Image.FileName);

                timelineViewModels.Add(foo);                
            }


            return View("AboutMe", timelineViewModels);
        }
    }
}
