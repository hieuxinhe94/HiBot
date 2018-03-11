using Autofac;
using HiBot.Business.Infrastructures;
using HiBot.Business.Interfaces;
using HiBot.Dialogs;
using HiBot.Entities;
using HiBot.Repository;
using HiBot.Repository.Base;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
 

namespace HiBot
{
    public class HiBotModule  : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            
            builder.RegisterType<StudentBusiness>()
                .Keyed<IStudentBusiness>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<StudentRepository>()
                .Keyed<IRepository<Students>>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();
            base.Load(builder);
        }
    }
   
}