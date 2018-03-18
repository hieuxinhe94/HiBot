using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using HiBot.Entities;
using HiBot.Repository.Base;
using HiBot.Repository.EntityFramework;
using HiBot.Repository.Infrastructure;

namespace HiBot.Repository
{
    [Serializable]
    public class BaseRepository<T> : IRepository<T> where T : BaseEntities
    {

        #region Fields
        private readonly HiBotDbContext _context;
        private DbSet<T> _entities;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion


        public BaseRepository()
        {
            _context = _context ?? new HiBotDbContext();
        }


        public virtual T GetById(object id)
        {
            // mention suggest to optimazation
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.Entities.Find(id);
        }

        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);
                this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                //context.RollBack();
                Console.WriteLine(dbEx.Message);
                throw;
            }
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this.Entities.AddRange(entities);

                this._context.SaveChanges();
            }
            catch (DbUpdateException  )
            {
                throw;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Attach(entity);
                this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw;
            }
        }

        public void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var item in entities)
                {
                    this.Entities.Attach(item);
                }
                this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (entity.Id < 0)
                {
                    return;
                }
                this.Entities.Remove(entity);

                this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(dbEx.Message);
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.Entities.Remove(entity);

                this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(dbEx.Message);
            }
        }

        public void Load()
        {
            this._context.Set<T>();
        }

        public IQueryable<T> FromSql(string sql, params object[] parameters)
        {
            return this.Entities.SqlQuery(sql, parameters).AsQueryable();
        }


        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return this.Entities.Where(whereCondition).ToList();
        }

        public IQueryable<T> GetQueryable()
        {
            return this.Entities.AsNoTracking();
        }

        public long Count(Expression<Func<T, bool>> expression)
        {
            return this.Entities.AsNoTracking().Where(expression).Count();
        }
    }
}