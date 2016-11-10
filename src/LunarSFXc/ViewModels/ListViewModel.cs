using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository repo, ICloudStorageService cloudService, int p)
        {
            Posts = Mapper.Map<ICollection<PostViewModel>>(repo.Posts(p - 1, 10));
            GetImageUris(Posts, cloudService);
            TotalPosts = repo.TotalPosts();
        }


        public ListViewModel(IBlogRepository repo, ICloudStorageService cloudService, string text, string type, int p)
        {
            switch (type)
            {
                case "Tag":
                    Posts = Mapper.Map<ICollection<PostViewModel>>(repo.PostsForTag(text, p - 1, 10));
                    GetImageUris(Posts, cloudService);
                    TotalPosts = repo.TotalPostsForTag(text);
                    Tag = Mapper.Map<TagViewModel>(repo.Tag(text));
                    break;
                case "Category":
                    Posts = Mapper.Map<ICollection<PostViewModel>>(repo.PostsForCategory(text, p - 1, 10));
                    GetImageUris(Posts, cloudService);
                    TotalPosts = repo.TotalPostsForCategory(text);
                    Category = Mapper.Map<CategoryViewModel>(repo.Category(text));
                    break;
                default:
                    Posts = Mapper.Map<ICollection<PostViewModel>>(repo.PostsForSearch(text, p - 1, 10));
                    GetImageUris(Posts, cloudService);
                    TotalPosts = repo.TotalPostsForSearch(text);
                    Search = text;
                    break;
            }
        }
        public ICollection<PostViewModel> Posts { get; set; }
        public int TotalPosts { get; set; }
        public CategoryViewModel Category { get; set; }
        public TagViewModel Tag { get; set; }
        public string Search { get; set; }
        
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
