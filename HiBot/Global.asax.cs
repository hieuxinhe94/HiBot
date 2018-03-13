using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using HiBot.Dialogs;
using HiBot.Dialogs.Common;
using HiBot.Midware;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;

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

            builder
                .RegisterType<LearningEnglishDialog>()
                .InstancePerDependency();

            builder.RegisterModule(new HiBotModule());

            builder
                .Register(c => new HelpDialog(c.Resolve<IDialogTask>()))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

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
