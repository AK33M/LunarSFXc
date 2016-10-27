using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class AboutMeViewModel
    {
        public AboutMeViewModel(ICloudStorageService cloudService, IBlogRepository repo)
        {
            Events = Mapper.Map<ICollection<TimelineEventViewModel>>(repo.GetTimelineEvents("aboutMe").Result);

            foreach(var ev in Events)
            {
                ev.Image.ImageUri = cloudService.GetImageUri(ev.Image.ContainerName, ev.Image.FileName);
            }
        }

        public AboutMeViewModel(ICloudStorageService cloudService, IBlogRepository repo, int id)
        {
            Events = Mapper.Map<ICollection<TimelineEventViewModel>>(repo.GetTimelineEvents("aboutMe", id).Result);

            foreach (var ev in Events)
            {
                ev.Image.ImageUri = cloudService.GetImageUri(ev.Image.ContainerName, ev.Image.FileName);
            }
        }

        public ICollection<TimelineEventViewModel> Events { get; set; }
    }
}
