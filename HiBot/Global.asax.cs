using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using HiBot.Dialogs;
using HiBot.Midware;
using Microsoft.Bot.Builder.Dialogs.Internals;

namespace HiBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

          
            var config = GlobalConfiguration.Configuration;


            // dependency injection
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new DialogModule());
        

            builder
                .RegisterType<RootDialog>()
                .InstancePerDependency();
            builder.RegisterModule(new HiBotModule());

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ServiceResolver.Container = container;
   
        }
        public static ILifetimeScope FindContainer()
        {
            var config = GlobalConfiguration.Configuration;
            var resolver = (AutofacWebApiDependencyResolver)config.DependencyResolver;
            return resolver.Container;
        }
    }
}
