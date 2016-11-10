using System.Collections.Generic;
using LunarSFXc.Objects;
using LunarSFXc.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
                                .Include(p => p.Images)
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
                                        .OrderByDescending(p => p.Id)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
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

        public int TotalPosts(bool checkIsPublished = true)
        {
            return _context.Posts
                             .Count(p => !checkIsPublished || p.Published);
        }

        public async Task<ImageDescription> GetFileDescription(int id)
        {
            return await _context.ImageDescriptions.SingleOrDefaultAsync(c => c.Id == id);
        }

        public void AddOrUpdateFileDescriptions(ImageDescription file)
        {
            if (_context.ImageDescriptions.Any(x => x.FileName == file.FileName && x.ContainerName == file.ContainerName))
            {
                var oldFile = _context.ImageDescriptions.FirstOrDefault(x => x.FileName == file.FileName && x.ContainerName == file.ContainerName);
                oldFile.UpdatedTimestamp = DateTime.UtcNow;
                _context.ImageDescriptions.Update(oldFile);
                file = oldFile;
            }
            else
            {
                _context.ImageDescriptions.Add(file);
            }

            _context.SaveChanges();

            //return file;
        }

        public async Task<ICollection<TimelineEvent>> GetTimelineEvents(string sectionName)
        {
            return await _context.TimelineEvents
                            .Include(x => x.Image)
                            .ToListAsync();
        }

        public async Task<ICollection<TimelineEvent>> GetTimelineEvents(string sectionName, int id)
        {
            return await _context.TimelineEvents
                            .Include(x => x.Image)
                            .Where(x => x.Id == id).ToListAsync();
        }

        public void AddOrUpdateTimelineEvent(TimelineEvent newEvent)
        {
            try
            {
                AddOrUpdateFileDescriptions(newEvent.Image);

                if (_context.TimelineEvents.Any(x => x.Id == newEvent.Id))
                {
                    _context.TimelineEvents.Attach(newEvent);
                    var oldEvent = _context.TimelineEvents.FirstOrDefault(x => x.Id == newEvent.Id);
                    oldEvent = newEvent;
                    _context.TimelineEvents.Update(oldEvent);
                }
                else
                {
                    _context.TimelineEvents.Add(newEvent);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error", ex);
                throw ex;
            }
        }

        public void AddOrUpdateBlogPost(Post post, LunarUser user)
        {
            try
            {
                AddOrUpdateChildEntities(post);

                if (_context.Posts.Any(x => x.Id == post.Id))
                {
                    _context.Posts.Attach(post);
                    var oldPost = _context.Posts.FirstOrDefault(x => x.Id == post.Id);

                    //Almost there!!!!!!!!!! PostTags Not updating.
                    oldPost.PostTags = post.PostTags;
                    oldPost.Images = post.Images;
                    oldPost.PostedBy = user;
                    oldPost.Modified = DateTime.Now;
                    _context.Posts.Update(oldPost);
                }
                else
                {
                    post.PostedBy = user;
                    post.PostedOn = DateTime.Now;
                    _context.Posts.Add(post);
                }


                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error", ex);
                throw ex;
            }
        }

        private void AddOrUpdateChildEntities(Post post)
        {

            //Gets the Id of the Post, Why you dont just use the Id in the first place is beyond me. You like to punish yourself!
            var postId = _context.Posts.AsNoTracking().FirstOrDefault(x => x.PostedOn.Year == post.PostedOn.Year &&
                                                                x.PostedOn.Month == post.PostedOn.Month &&
                                                                 x.UrlSlug == post.UrlSlug) == null ? 0 : _context.Posts.AsNoTracking().FirstOrDefault(x => x.PostedOn.Year == post.PostedOn.Year &&
                                                                x.PostedOn.Month == post.PostedOn.Month &&
                                                                 x.UrlSlug == post.UrlSlug).Id;
            post.Id = postId;


            post.Category = _context.Categories.FirstOrDefault(x => x.Id == post.CategoryId);

            //Tags
            var newTags = new List<PostTag>();

            foreach (var pt in post.PostTags)
            {
                pt.PostId = postId;
                //IsNotNEW
                if (_context.Tags.Any(x => x.Name == pt.Tag.Name && x.UrlSlug == pt.Tag.UrlSlug))
                {
                    var oldTag = _context.Tags.AsNoTracking().FirstOrDefault(x => x.Name == pt.Tag.Name && x.UrlSlug == pt.Tag.UrlSlug);
                    pt.TagId = oldTag.Id;
                    pt.Tag = oldTag;

                    newTags.Add(_context.PostTags.AsNoTracking().SingleOrDefault(x => x.PostId == pt.PostId && x.TagId == pt.TagId));

                    if (!_context.PostTags.Any(x => x.PostId == pt.PostId && x.TagId == pt.TagId))
                    {
                        pt.Post = null;
                        pt.Tag = null;
                        _context.PostTags.Add(pt);
                    }
                }
                else //IsNew
                {
                    _context.Tags.Add(pt.Tag);
                }
            }

            var oldPost = _context.Posts.AsNoTracking()
                                        .Include(x => x.PostTags)
                                        .Include(x => x.Images)
                                        .SingleOrDefault(x => x.Id == postId);



            var removedTags = oldPost?.PostTags.Except(newTags, new PostTagComparer()).ToList();

            if (removedTags != null && removedTags.Any())
            {
                foreach (var tag in removedTags)
                {
                    tag.Post = null;
                    tag.Tag = null;
                }
                _context.PostTags.RemoveRange(removedTags);
            }


            //Images
            var images = new List<ImageDescription>();

            foreach (var image in post.Images)
            {
                images.Add(_context.ImageDescriptions.SingleOrDefault(x => x.FileName == image.FileName &&
                                                                   x.ContainerName == image.ContainerName));
            }

            post.Images = images;
            var removedImages = oldPost.Images.Except(images, new ImageDescriptionComparer()).ToList();

            if (removedImages != null && removedImages.Any())
                _context.ImageDescriptions.RemoveRange(removedImages);

            _context.ImageDescriptions.UpdateRange(post.Images);
        }

        public async Task<ICollection<ImageDescription>> GetAllImages(string containerName)
        {
            return await _context.ImageDescriptions.Where(x => x.ContainerName == containerName).ToListAsync();
        }

        public async Task<int> DeleteTimelineEvent(int id)
        {
            try
            {
                var timelineEvent = await _context.TimelineEvents.FirstOrDefaultAsync(x => x.Id == id);

                _context.Remove(timelineEvent);

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error", ex);
                throw ex;
            }
        }

        public void AddOrUpdateCategory(Category cat)
        {
            try
            {
                _context.Categories.Add(cat);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error", ex);
                throw;
            }
        }
    }
}
