using Autofac;
using LunarSFXc.Repositories;

namespace LunarSFXc.Autofac
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //repos here!
            builder.RegisterType<BlogRepository>().As<IBlogRepository>();
        }
    }
}