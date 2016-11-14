using LunarSFXc.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace LunarSFXc.Repositories
{
    public interface IBlogRepository
    {
        ICollection<Post> Posts(int pageNo, int pageSize);
        int TotalPosts(bool checkIsPublished = true);
        ICollection<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        ICollection<Project> GetAllProjects();
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);
        ICollection<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        void AddOrUpdateComment(Comment comment);
        Category Category(string categorySlug);
        ICollection<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);
        Post Post(int year, int month, string titleSlug);
        ICollection<Comment> GetChildComments(int parentCommentId);
        Comment Comment(int parentCommentId);
        ICollection<Category> Categories();
        ICollection<Tag> Tags();

        //About Me
        Task<ICollection<TimelineEvent>> GetTimelineEvents(string sectionName);
        Task<ICollection<TimelineEvent>> GetTimelineEvents(string sectionName, int id);

        //Admin features
        ICollection<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending);
        Task<ImageDescription> GetFileDescription(int id);
        void AddOrUpdateFileDescriptions(ImageDescription file);
        void AddOrUpdateTimelineEvent(TimelineEvent newEvent);

        Task<ICollection<ImageDescription>> GetAllImages(string containerName);
        Task<int> DeleteTimelineEvent(int id);
        void AddOrUpdateBlogPost(Post post, LunarUser user);
        void AddOrUpdateCategory(Category cat);
        void DeleteComment(Comment id);
    }
}
