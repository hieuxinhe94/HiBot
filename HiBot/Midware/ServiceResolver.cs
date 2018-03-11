using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace HiBot.Midware
{
    public class ServiceResolver
    {
        public static IContainer Container;

        public static T Get<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
                return scope.Resolve<T>();
        }
    }
}