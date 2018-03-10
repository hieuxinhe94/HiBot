using System.Data.Entity.Core.Objects;

namespace HiBot.Repository.Base
{
    // context across all repository - global repository
    public interface IRepositoryContext
    {
        IObjectSet<T> GetObjectSet<T>() where T : class;
        ObjectContext ObjectContext { get; }

        // save all changes of all repository
        int SaveChange();
        // terminate all repository in current context
        void Terminate();
    }
}
