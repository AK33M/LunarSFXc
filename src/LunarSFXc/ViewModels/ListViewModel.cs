using AutoMapper;
using LunarSFXc.Repositories;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository repo, int p)
        {
            Posts = Mapper.Map<ICollection<PostViewModel>>(repo.Posts(p - 1, 10));
            TotalPosts = repo.TotalPosts();
        }

        public ListViewModel(IBlogRepository repo, string text, string type, int p)
        {
            switch (type)
            {
                case "Tag":
                    Posts = Mapper.Map<ICollection<PostViewModel>>(repo.PostsForTag(text, p - 1, 10));
                    TotalPosts = repo.TotalPostsForTag(text);
                    Tag = Mapper.Map<TagViewModel>(repo.Tag(text));
                    break;
                case "Category":
                    Posts = Mapper.Map<ICollection<PostViewModel>>(repo.PostsForCategory(text, p - 1, 10));
                    TotalPosts = repo.TotalPostsForCategory(text);
                    Category = Mapper.Map<CategoryViewModel>(repo.Category(text));
                    break;
                default:
                    Posts = Mapper.Map<ICollection<PostViewModel>>(repo.PostsForSearch(text, p - 1, 10));
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
    }
}
