using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HiBot.Business.Interfaces;
using HiBot.Entities;
using HiBot.Repository;
using HiBot.Repository.Base;

namespace HiBot.Business.Infrastructures
{
    [Serializable]
    public class StudentBusiness : IStudentBusiness
    {
        private readonly IRepository<Students> _studentRepository;

         
        public StudentBusiness(IRepository<Students> studentRepository)
        {
            _studentRepository = studentRepository ?? new  StudentRepository();
        }

        public async Task<ICollection<Students>> GetAll()
        {
            return  _studentRepository.TableNoTracking.ToList();
        }

        public ICollection<Students> GetByEpression(Expression<Func<Students, bool>> express)
        {
            return _studentRepository.GetAll(express).ToList();
        }

        public Students GetSingle(int id)
        {
            return _studentRepository.GetById(id);
        }

        public int Add(Students student)
        {
             _studentRepository.Insert(student);
            return 1;
        }
    }
}
