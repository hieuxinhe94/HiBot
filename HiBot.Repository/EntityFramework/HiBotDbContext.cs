using System.Data.Entity;
using HiBot.Entities;

namespace HiBot.Repository.EntityFramework
{
    public class HiBotDbContext : DbContext
    {
        public HiBotDbContext() : base(@"data source=localhost\SQLEXPRESS;
        initial catalog = HiBot;
        User=sa;
        Password=12345678;")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>().ToTable("Students");
            modelBuilder.Entity<Teachers>().ToTable("Teachers");
            modelBuilder.Entity<CollegeStudent>().ToTable("CollegeStudent");


            base.OnModelCreating(modelBuilder);


        }


        public DbSet<Students> Students { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<CollegeStudent> CollegeStudent { get; set; }
    }
}
