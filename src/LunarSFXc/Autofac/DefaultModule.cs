using Autofac;
using LunarSFXc.Contexts;
using LunarSFXc.Extensions;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LunarSFXc.Autofac
{
    public class DefaultModule : Module
    {
        public IHostingEnvironment _env;

        protected override void Load(ContainerBuilder builder)
        {
            //repos here!
            builder.RegisterType<BlogRepository>().As<IBlogRepository>();
            
            builder.RegisterType<MessageSender>().As<IEmailService>();

            builder.RegisterType<MessageSender>().As<ISmsService>();

            builder.RegisterType<CloudStorageService>().As<ICloudStorageService>();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

            builder.RegisterType<ValidateMimeMultipartContentFilter>().InstancePerLifetimeScope();

            if (_env.IsDevelopment())
            {
                //builder.RegisterType<DebugMailService>().As<IEmailService>();
            }
        }
    }
}