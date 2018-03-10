using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using HiBot.Repository.Base;
using HiBot.Repository.EntityFramework;
using HiBot.Repository.Infrastructure;

namespace HiBot.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private HiBotDbContext hiBotDbContext;
        public BaseRepository() : this(new HiBotContext())
        {
             
        }

        public BaseRepository(IRepositoryContext repositoryContext)
        {
            repositoryContext = repositoryContext ?? new HiBotContext();
            _objectSet = repositoryContext.GetObjectSet<T>();
        }

        private IObjectSet<T> _objectSet;

        public IObjectSet<T> ObjectSet
        {
            get { return _objectSet; }
        }

        #region IRepository Members

        public int Add(T entity)
        {
            this.ObjectSet.AddObject(entity);
            
            return 1;
        }

        public int AddMany(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(T entity)
        {
             this.ObjectSet.DeleteObject(entity);
            return 1;
        }

        public int UpdateSingle(T entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateMany(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> TableNoTracking()
        {
            return ObjectSet;
        }

        public IList<T> GetAll()
        {
            return this.ObjectSet.ToList<T>();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        =>
            this.ObjectSet.Where(whereCondition).ToList();

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).FirstOrDefault<T>();
        }

        public int AddSingle(T entity)
        {
            throw new NotImplementedException();
        }

        public void Attach(T entity)
        {
            this.ObjectSet.Attach(entity);
        }

        public IQueryable<T> GetQueryable()
        {
            return this.ObjectSet.AsQueryable<T>();
        }

        public long Count()
        {
            return this.ObjectSet.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).LongCount<T>();
        }

        #endregion
    }
}