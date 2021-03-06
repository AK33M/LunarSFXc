﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LunarSFXc.Contexts;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LunarSFXc.Autofac;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LunarSFXc.Objects;
using LunarSFXc.Services;
using AutoMapper;
using LunarSFXc.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LunarSFXc
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                            .SetBasePath(_env.ContentRootPath)
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();

            if (_env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            _config = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            //Add your Services here..
            services.AddMvc(config =>
            {
                //if (_env.IsProduction())
                //    config.Filters.Add(new RequireHttpsAttribute());
            });
            services.AddLogging();
            services.AddIdentity<LunarUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<LunarDbContext>()
            .AddDefaultTokenProviders();           

            services.AddDbContext<LunarDbContext>(options => options.UseSqlServer(_config.GetConnectionString("AkeemTaiwoConn")));
            services.Configure<EmailSenderOptions>(_config);
            services.Configure<ImageServiceOptions>(_config);
            //services.AddTransient<SampleSeedData>();


            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DefaultModule() { _env = _env });
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, RoleManager<IdentityRole> roleManager)
        {
            //loggerFactory.AddElmahIo(_config.GetValue<string>("ElmahIo:API_KEY"), new Guid(_config.GetValue<string>("ElmahIo:LOG_ID")));
            
            ConfigureRoles(roleManager);                      

            loggerFactory.AddConsole();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //await seeder.EnsureSeedDataAsync();

                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseFileServer();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Post, PostViewModel>()
                                        .ForMember(dest => dest.Tags, opt => opt.MapFrom(x => x.PostTags))
                                        .ReverseMap();

                config.CreateMap<PostViewModel, Post>()
                        .AfterMap((pv, p) =>
                        {
                            p.PostTags = new List<PostTag>();

                            foreach (var item in pv.Tags)
                            {
                                p.PostTags.Add(new PostTag()
                                {
                                    //Post = p,
                                    PostId = p.Id,
                                    Tag = Mapper.Map<Tag>(item),
                                });
                            }
                        });

                config.CreateMap<TagViewModel, Tag>().ReverseMap();
                config.CreateMap<CategoryViewModel, Category>().ReverseMap();
                config.CreateMap<Comment, CommentViewModel>()
                                    .ForMember(dest => dest.User, opts => opts.MapFrom(s => s.Owner.UserName))
                                    .ForMember(dest => dest.ProfileImageContainerName, opts => opts.MapFrom(s => s.Owner.ProfileImage.ContainerName))
                                    .ForMember(dest => dest.ProfileImageFileName, opts => opts.MapFrom(s => s.Owner.ProfileImage.FileName))
                                    .ForMember(dest => dest.PostTitle, opts => opts.MapFrom(s => s.ParentPost.UrlSlug))
                                    .ForMember(dest => dest.Year, opts => opts.MapFrom(s => s.ParentPost.PostedOn.Year))
                                    .ForMember(dest => dest.Month, opts => opts.MapFrom(s => s.ParentPost.PostedOn.Month))
                                    .ReverseMap();
                config.CreateMap<TimelineEvent, TimelineEventViewModel>()
                                        .ReverseMap();
                config.CreateMap<ImageDescriptionViewModel, ImageDescription>()
                                        .ReverseMap();

                config.CreateMap<PostTag, TagViewModel>()
                        .ForMember(dest => dest.Name, opts => opts.MapFrom(s => s.Tag.Name))
                        .ForMember(dest => dest.Description, opts => opts.MapFrom(s => s.Tag.Description))
                        .ForMember(dest => dest.UrlSlug, opts => opts.MapFrom(s => s.Tag.UrlSlug))
                        .ReverseMap();

                config.CreateMap<LunarUser, LunarUserViewModel>()
                .AfterMap((l, lv) =>
                {
                    lv.Roles = new List<string>();

                    var role = new IdentityRole();
                    foreach (var item in l.Roles)
                    {
                        role = roleManager.FindByIdAsync(item.RoleId).Result;
                        lv.Roles.Add(role.Name);
                    }
                })
                .ReverseMap();

                config.CreateMap<ProjectViewModel, Project>().ReverseMap();
            });

            //Use MVC always Last.
            app.UseMvc(ConfigureRoutes);
        }

        private void ConfigureRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            else if (!roleManager.RoleExistsAsync("Contributor").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Contributor"));
            }
            else if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
            {
                roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            else if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                roleManager.CreateAsync(new IdentityRole("NormalUser"));
            }

        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                        "Admin",
                        "Admin/{action}",
                        new { controller = "Admin", action = "Index" }
                    );

            routeBuilder.MapRoute(
                        "Tag",
                        "Tag/{tag}",
                        new { controller = "Blog", action = "Tag" }
                    );

            routeBuilder.MapRoute(
                        "Post",
                        "Archive/{year}/{month}/{title}",
                        new { controller = "Blog", action = "Post" }
                    );

            routeBuilder.MapRoute(
                        "Category",
                        "Category/{category}",
                        new { controller = "Blog", action = "Category" }
                    );

            routeBuilder.MapRoute(
                        "Auth",
                         "Auth/{action}",
                        new { controller = "Auth", action = "Login" }
                    );

            routeBuilder.MapRoute(
                        "Action",
                        "Blog/{action}",
                        new { controller = "Blog", action = "Posts" }
                    );

            routeBuilder.MapRoute(
                       "Main",
                       "{id?}",
                       new { controller = "Home", action = "Index" }
                   );

            routeBuilder.MapRoute(
                    name: "Contact",
                    template: "Contact/Send",
                    defaults: new { controller = "Contact", action = "Send" });

            routeBuilder.MapRoute(
                    name: "Comment",
                    template: "Comment/{action}",
                    defaults: new { controller = "Comment", action = "Save" });

            //routeBuilder.MapRoute(
            //        name: "Default",
            //        template: "{controller=Home}/{action=Index}/{id?}");

        }
    }
}
