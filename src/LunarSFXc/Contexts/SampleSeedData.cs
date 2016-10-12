using LunarSFXc.Objects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunarSFXc.Contexts
{
    public class SampleSeedData
    {
        private LunarDbContext _context;
        private UserManager<LunarUser> _userManager;

        public SampleSeedData(LunarDbContext context, UserManager<LunarUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            if(await _userManager.FindByEmailAsync("lunar@lunarsfx.com") == null)
            {
                var user = new LunarUser
                {
                    Email = "lunar@lunarsfx.com",
                    FirstWords = "Every one needs a hobby",
                    UserName = "Lunar1",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user, "P@ssw0rd!");
            }

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
                        },
                        PostedBy = await _userManager.FindByEmailAsync("lunar@lunarsfx.com")
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
