using LunarSFXc.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarSFXc.Contexts
{
    public class SampleSeedData
    {
        private LunarDbContext _context;

        public SampleSeedData(LunarDbContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (!_context.Posts.Any())
            {
                var sampleTags = new List<Tag>
            {
               new Tag {Description= "About Travel",  Name= "Travel", UrlSlug="travel" },
               new Tag {Description= "About Careers",  Name= "Career", UrlSlug="career" },
               new Tag {Description= "About Programming",  Name= "Programming", UrlSlug="programming" },
               new Tag {Description= "About Lifestyle",  Name= "Lifestyle", UrlSlug="lifestyle" },
               new Tag {Description= "About ASP.NET",  Name= "ASP.NET", UrlSlug="asp_net" },
            };

                var sampleCategory = new Category
                {
                    Description = "Some posts about Home Improvement",
                    Name = "Home Improvement",
                    UrlSlug = "home_improvement"
                };

                var sampleCategory2 = new Category
                {
                    Description = "Some posts about Software Development",
                    Name = "Software Development",
                    UrlSlug = "software_development"
                };

                var samplePosts = new List<Post>
            {
                new Post {
                    Title ="Building this site",
                    UrlSlug ="building_this_site",
                    Description ="Step by step process of builing this site.",
                    Category = sampleCategory2,
                    Meta ="mvc, asp.net",
                    Published = true,
                    ShortDescription = "Step by Step process about building the site.",
                    PostedOn =DateTime.Today,
                    PostTags = new List<PostTag>
                    {
                        new PostTag {Tag = sampleTags.FirstOrDefault()},
                        new PostTag {Tag = sampleTags.LastOrDefault()}
                    }
                }
            };

                _context.Tags.AddRange(sampleTags);
                _context.Categories.AddRange(sampleCategory, sampleCategory2);
                _context.Posts.AddRange(samplePosts);

                await _context.SaveChangesAsync();
            }            
        }
    }
}
