using Autofac;
using HiBot.Dialogs.Students;

namespace HiBot.App_Start
{
    public class StudentServeyModuln : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register((c, p) => new StudentServeyDialog()).AsSelf().InstancePerDependency();
        }
    }
}