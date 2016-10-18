using Autofac;
using LunarSFXc.Contexts;
using LunarSFXc.Extensions;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using Microsoft.AspNetCore.Hosting;

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

            builder.RegisterType<ValidateMimeMultipartContentFilter>().InstancePerLifetimeScope();

            if (_env.IsDevelopment())
            {
                //builder.RegisterType<DebugMailService>().As<IEmailService>();
            }
        }
    }
}