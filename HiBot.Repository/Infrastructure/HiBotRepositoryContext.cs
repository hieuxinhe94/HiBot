using System.Data.Entity.Core.Objects;
using HiBot.Repository.Base;

namespace HiBot.Repository.Infrastructure
{
    public class HiBotRepositoryContext : IRepositoryContext
    {
        private const string OBJECT_CONTEXT_KEY = "HiBot.Dal.EF.Entities";
        public IObjectSet<T> GetObjectSet<T>() where T : class
        {
            return ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY).CreateObjectSet<T>();
        }

        public ObjectContext ObjectContext
        {
            get =>
                 ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY);

        }

        public int SaveChange() => this.ObjectContext.SaveChanges();


        public void Terminate() => ContextManager.SetRepositoryContext(null, OBJECT_CONTEXT_KEY);

    }
}
