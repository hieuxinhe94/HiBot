using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using HiBot.Business.Infrastructures;
using HiBot.Business.Interfaces;
using HiBot.Dialogs;
using HiBot.Repository;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace HiBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //var uri = new Uri(ConfigurationManager.AppSettings["DocumentDbUrl"]);
            //var key = ConfigurationManager.AppSettings["DocumentDbKey"];
            //var store = new DocumentDbBotDataStore(uri, key);

            var config = GlobalConfiguration.Configuration;


            // dependency injection
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new DialogModule());

            builder.RegisterModule(new HiBotModule());
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);


        }
        public static ILifetimeScope FindContainer()
        {
            var config = GlobalConfiguration.Configuration;
            var resolver = (AutofacWebApiDependencyResolver)config.DependencyResolver;
            return resolver.Container;
        }
    }
}
