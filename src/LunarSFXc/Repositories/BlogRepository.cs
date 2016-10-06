using System.Collections.Generic;
using LunarSFXc.Objects;
using LunarSFXc.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LunarSFXc.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly LunarDbContext _context;

        public BlogRepository(LunarDbContext context)
        {
            _context = context;
        }
        public ICollection<Post> Posts(int pageNo, int pageSize)
        {
            //check if it includes Category and Tags
            var posts = _context.Posts
                                .Where(p => p.Published)
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                .ToList();

            GetTags(posts);

            return posts;

        }

        public ICollection<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = _context.Posts
                                .Where(p => p.Published && p.PostTags.Any(t => t.Tag.UrlSlug.Equals(tagSlug)))
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                .ToList();

            GetTags(posts);

            return posts;
        }

        private void GetTags(List<Post> posts)
        {
            foreach (var post in posts)
            {
                if (post.PostTags != null)
                {
                    foreach (var tag in post.PostTags)
                    {
                        //get tags
                        tag.Tag = _context.Tags.FirstOrDefault(t => t.Id == tag.TagId);
                    }
                }
            }
        }

        public Tag Tag(string tagSlug)
        {
            var tag = _context.Tags
                                .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));

            return tag;
        }

        public int TotalPosts()
        {
            return _context.Posts
                            .Where(p => p.Published)
                            .Count();
        }

        public int TotalPostsForTag(string tagSlug)
        {
            return _context.Posts
                            .Where(p => p.Published && p.PostTags.Any(t => t.Tag.UrlSlug.Equals(tagSlug)))
                            .Count();
        }

        public ICollection<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = _context.Posts
                                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                .ToList();

            GetTags(posts);
            return posts;
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            return _context.Posts.Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .Count();
        }

        public Category Category(string categorySlug)
        {
            return _context.Categories.FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }

        public ICollection<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = _context.Posts
                                .Where(p => p.Published && (p.Title.Contains(search)
                                                            || p.Category.Name.Equals(search)
                                                            || p.PostTags.Any(t => t.Tag.Name.Equals(search))))
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                .ToList();

            GetTags(posts);

            return posts;

        }

        public int TotalPostsForSearch(string search)
        {
            return _context.Posts
                           .Where(p => p.Published && (p.Title.Contains(search)
                                                        || p.Category.Name.Equals(search)
                                                        || p.PostTags.Any(t => t.Tag.Name.Equals(search))))
                           .Count();
        }

        public Post Post(int year, int month, string titleSlug)
        {
            var query = _context.Posts
                                .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                .ToList();

            GetTags(query);

            return query.FirstOrDefault();
        }

        public ICollection<Category> Categories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Tag> Tags()
        {
            return _context.Tags.OrderBy(t => t.Name).ToList();
        }
    }
}
