using AutoMapper;
using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository repo)
        {
            Categories = Mapper.Map<ICollection<CategoryViewModel>>(repo.Categories());
            Tags = Mapper.Map<ICollection<TagViewModel>>(repo.Tags());
            LastestPosts = Mapper.Map<ICollection<PostViewModel>>(repo.Posts(0, 3));
        }

        public ICollection<CategoryViewModel> Categories { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
        public ICollection<PostViewModel> LastestPosts { get; set; }
    }
}
