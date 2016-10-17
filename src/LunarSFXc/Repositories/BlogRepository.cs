using System.Collections.Generic;
using LunarSFXc.Objects;
using LunarSFXc.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;

namespace LunarSFXc.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly LunarDbContext _context;
        private ILogger<BlogRepository> _logger;

        public BlogRepository(LunarDbContext context, ILogger<BlogRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        public ICollection<Post> Posts(int pageNo, int pageSize)
        {
            try
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
                                    //.Include(p => p.Comments)
                                    .ToList();

                GetTags(posts);

                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting all Posts from database", ex);
                return null;
            }
        }
        public ICollection<Category> Categories()
        {
            try
            {
                return _context.Categories.OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting all Categories from database", ex);
                return null;
            }
        }
        public ICollection<Tag> Tags()
        {
            try
            {
                return _context.Tags.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting all Tags from database", ex);
                return null;
            }
        }
        public ICollection<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            try
            {
                var posts = _context.Posts
                               .Where(p => p.Published && p.PostTags.Any(t => t.Tag.UrlSlug.Equals(tagSlug)))
                               .OrderByDescending(p => p.PostedOn)
                               .Skip(pageNo * pageSize)
                               .Take(pageSize)
                               .Include(p => p.Category)
                               .Include(p => p.PostTags)
                               .Include(p => p.PostedBy)
                               //.Include(p => p.Comments)
                               .ToList();

                GetTags(posts);

                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all Posts with Tag '{tagSlug}' from database", ex);
                return null;
            }

        }
        public ICollection<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            try
            {
                var posts = _context.Posts
                                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                //.Include(p => p.Comments)
                                .ToList();

                GetTags(posts);
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all Posts for Category '{categorySlug}' from database", ex);
                return null;
            }
        }
        public ICollection<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            try
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
                               //.Include(p => p.Comments)
                               .ToList();

                GetTags(posts);

                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all Posts with search term '{search}' from database", ex);
                return null;
            }
        }

        public ICollection<Comment> GetChildComments(int parentCommentId)
        {
            try
            {
                var parentComment = _context.Comments.Include(c => c.Replies).FirstOrDefault(c => c.Id == parentCommentId);

                var childComments = parentComment.Replies;

                return childComments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all child comments", ex);
                return null;
            }
        }

        public Tag Tag(string tagSlug)
        {
            try
            {
                var tag = _context.Tags
                                .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));

                return tag;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Tag '{tagSlug}' from database", ex);
                return null;
            }
        }
        public Post Post(int year, int month, string titleSlug)
        {
            try
            {
                var query = _context.Posts
                                .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .Include(p => p.PostedBy)
                                .Include(p => p.Comments)
                                .ToList();

                GetTags(query);

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Posts with title '{titleSlug}' from database", ex);
                return null;
            }
        }
        public Category Category(string categorySlug)
        {
            try
            {
                return _context.Categories.FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Category '{categorySlug}' from database", ex);
                return null;
            }
        }

        public int TotalPostsForSearch(string search)
        {
            return _context.Posts
                           .Where(p => p.Published && (p.Title.Contains(search)
                                                        || p.Category.Name.Equals(search)
                                                        || p.PostTags.Any(t => t.Tag.Name.Equals(search))))
                           .Count();
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
        public int TotalPostsForCategory(string categorySlug)
        {
            return _context.Posts.Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .Count();
        }

        private void GetTags(ICollection<Post> posts)
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

        //Admin Features
        public ICollection<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending)
        {
            ICollection<Post> posts;
            // ICollection<int> postIds;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    break;
                case "Published":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    break;
                case "Category":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    break;
                default:
                    //Order by Id
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Id)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    else
                    {
                        posts = _context.Posts
                                        //.OrderByDescending(p => p.Id)
                                        //.Skip(pageNo * pageSize)
                                        //.Take(pageSize)
                                        .Include(p => p.PostTags)
                                        .Include(p => p.Category)
                                        .Include(p => p.PostedBy)
                                        .ToList();

                        GetTags(posts);
                    }
                    break;
            }

            return posts;
        }




    }
}
