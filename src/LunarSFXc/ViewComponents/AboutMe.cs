using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarSFXc.ViewComponents
{
    public class AboutMe : ViewComponent
    {
        private IBlogRepository _repo;

        public AboutMe(IBlogRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _repo.GetTimelineEvents("aboutMe");
            var timelineViewModels = new List<TimelineEventViewModel>();

            foreach(var ev in events)
            {
                timelineViewModels.Add(Mapper.Map<TimelineEventViewModel>(ev));
            }


            return View("AboutMe", timelineViewModels);
        }
    }
}
