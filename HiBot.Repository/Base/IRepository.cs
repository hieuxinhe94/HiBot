using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HiBot.Repository.Base
{
    // Generic type
   public interface IRepository<T>
    {
        /// <summary>
        /// Lấy một đối tượng bởi primary key hoặc biểu thức unique nào đó
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        T GetSingle(Expression<Func<T,bool>> whereCondition );

        int AddSingle(T entity);
        int AddMany(List<T> entities);
        int Delete(T entity);
        int UpdateSingle(T entity);
        int UpdateMany(T entity);
        IEnumerable<T> TableNoTracking();
        IList<T> GetAll();
        /// <summary>
        /// input param is T , return bool 
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        IList<T> GetAll(Expression<Func<T,bool>> whereCondition);
        IQueryable<T> GetQueryable();
        long Count(Expression<Func<T, bool>> expression);

    }
}
