using LunarSFXc.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LunarSFXc.Contexts
{
    public class LunarDbContext : DbContext
    {
        public LunarDbContext(IConfigurationRoot config, DbContextOptions<LunarDbContext> options)
                : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
