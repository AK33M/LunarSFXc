using Autofac;
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

            if (_env.IsDevelopment())
            {
                builder.RegisterType<DebugMailService>().As<IMailService>();
            }
            {
                //Insert Real mail service here.. Perhaps Google?
            }
        }
    }
}