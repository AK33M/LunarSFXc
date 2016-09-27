using Autofac;
using LunarSFXc.Contexts;
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
                builder.RegisterType<SampleSeedData>();
            }
            else
            {
                //Insert Real mail service here.. Perhaps Google?
            }
        }
    }
}