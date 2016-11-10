using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository repo, ICloudStorageService cloudService)
        {
            Categories = Mapper.Map<ICollection<CategoryViewModel>>(repo.Categories());
            Tags = Mapper.Map<ICollection<TagViewModel>>(repo.Tags());
            LastestPosts = Mapper.Map<ICollection<PostViewModel>>(repo.Posts(0, 3));
            GetImageUris(LastestPosts, cloudService);
        }

        public ICollection<CategoryViewModel> Categories { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
        public ICollection<PostViewModel> LastestPosts { get; set; }

        private void GetImageUris(ICollection<PostViewModel> posts, ICloudStorageService cloudService)
        {
            foreach (var post in posts)
            {
                foreach (var img in post.Images)
                {
                    img.ImageUri = cloudService.GetImageUri(img.ContainerName, img.FileName);
                }
            }
        }
    }
}
