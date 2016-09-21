using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using LunarSFXc.Contexts;

namespace LunarSFXc.Migrations
{
    [DbContext(typeof(LunarDbContext))]
    partial class LunarDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LunarSFXc.Objects.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("UrlSlug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LunarSFXc.Objects.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Meta")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 1000);

                    b.Property<DateTime?>("Modified");

                    b.Property<DateTime>("PostedOn");

                    b.Property<bool>("Published");

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("UrlSlug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("LunarSFXc.Objects.PostTag", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("TagId");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("LunarSFXc.Objects.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("UrlSlug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("LunarSFXc.Objects.Post", b =>
                {
                    b.HasOne("LunarSFXc.Objects.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LunarSFXc.Objects.PostTag", b =>
                {
                    b.HasOne("LunarSFXc.Objects.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LunarSFXc.Objects.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
