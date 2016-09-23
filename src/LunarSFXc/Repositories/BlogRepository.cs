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
        public IList<Post> Posts(int pageNo, int pageSize)
        {
            //check if it includes Category and Tags
            var posts = _context.Posts
                                .Where(p => p.Published)
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .ToList();

            foreach(var post in posts)
            {
                foreach(var tag in post.PostTags)
                {
                    //get tags
                    tag.Tag = _context.Tags.FirstOrDefault(t => t.Id == tag.TagId);
                }
            }

            //var postIds = posts.Select(p => p.Id).ToList();

            return posts;

        }

        public int TotalPosts()
        {
            return _context.Posts
                            .Where(p => p.Published)
                            .Count();
        }
    }
}
