using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HiBot.Business.Interfaces;
using HiBot.Entities;
using HiBot.Repository.Base;

namespace HiBot.Business.Infrastructures
{
    public class StudentBusiness : IStudentBusiness
    {
        private readonly IRepository<Students> _studentRepository;

        public StudentBusiness(IRepository<Students> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public ICollection<Students> GetAll()
        {
            return _studentRepository.TableNoTracking().ToList();
        }

        public ICollection<Students> GetByEpression(Expression<Func<Students, bool>> express)
        {
            return _studentRepository.GetAll(express).ToList();
        }

        public Students GetSingle(int id)
        {
            return _studentRepository.TableNoTracking().SingleOrDefault(t => t.Id == id);
        }

        public int Add(Students student)
        {
            return _studentRepository.AddSingle(student);
        }
    }
}
