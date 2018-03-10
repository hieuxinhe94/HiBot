using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HiBot.Entities;

namespace HiBot.Business.Interfaces
{
    public interface IStudentBusiness
    {
        ICollection<Students> GetAll();
        ICollection<Students> GetByEpression(Expression<Func<Students,bool>> express);
        Students GetSingle(int id);

        int Add(Students student);
    }
}
