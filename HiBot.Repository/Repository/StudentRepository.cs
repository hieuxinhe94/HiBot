using System;
using HiBot.Entities;
using HiBot.Repository.EntityFramework;
using HiBot.Repository.Interfaces;

namespace HiBot.Repository
{
    [Serializable]
    public class StudentRepository : BaseRepository<Students>, IStudentRepository
    {
        // implement more function
       
    }
}
