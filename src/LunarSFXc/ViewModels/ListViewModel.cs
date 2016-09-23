using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository repo, int p)
        {
            Posts = repo.Posts(p - 1, 10);
            TotalPosts = repo.TotalPosts();
        }

        public ListViewModel(IBlogRepository repo, string text, string type, int p)
        {
            switch (type)
            {
                case "Tag":
                    Posts = repo.PostsForTag(text, p - 1, 10);
                    TotalPosts = repo.TotalPostsForTag(text);
                    Tag = repo.Tag(text);
                    break;
                case "Category":
                    Posts = repo.PostsForCategory(text, p - 1, 10);
                    TotalPosts = repo.TotalPostsForCategory(text);
                    Category = repo.Category(text);
                    break;
                default:
                    Posts = repo.PostsForSearch(text, p - 1, 10);
                    TotalPosts = repo.TotalPostsForSearch(text);
                    Search = text;
                    break;
            }
        }
        public ICollection<Post> Posts { get; set; }
        public int TotalPosts { get; set; }
        public Category Category { get; set; }
        public Tag Tag { get; set; }
        public string Search { get; set; }
    }
}
