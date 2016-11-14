using LunarSFXc.Objects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LunarSFXc.Contexts
{
    public class LunarDbContext : IdentityDbContext<LunarUser>
    {
        public LunarDbContext(IConfigurationRoot config, DbContextOptions<LunarDbContext> options)
                : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ImageDescription> ImageDescriptions { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             * Many-to-many relationships without an entity class to represent the join table are not yet supported. 
             * However, you can represent a many-to-many relationship by including an entity 
             * class for the join table and mapping two separate one-to-many relationships.
             */
            //modelBuilder.Entity<Post>()
            //           .HasMany(x => x.Tags)
            //           .WithMany(x => x.Posts)
            //           .Map(mc =>
            //           {
            //               mc.ToTable("PostTagMap");
            //               mc.MapLeftKey("PostId");
            //               mc.MapRightKey("TagId");
            //           });

            //modelBuilder.Entity<Comment>()
            //    .Property(c => c.Replies)
            //    .HasColumnName("ChildCommentId");

            modelBuilder.Entity<PostTag>().ToTable("PostTag");

            modelBuilder.Entity<PostTag>()
                .HasKey(t => new { t.PostId, t.TagId });

            //modelBuilder.Entity<PostTag>()
            //    .HasIndex(u => new { u.PostId, u.TagId }).IsUnique();

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
