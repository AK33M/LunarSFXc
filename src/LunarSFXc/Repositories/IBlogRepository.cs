using LunarSFXc.Objects;
using System.Collections.Generic;

namespace LunarSFXc.Repositories
{
    public interface IBlogRepository
    {
        ICollection<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();
        ICollection<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);
        ICollection<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);
        ICollection<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);
        Post Post(int year, int month, string titleSlug);
        ICollection<Category> Categories();
        ICollection<Tag> Tags();
    }
}
