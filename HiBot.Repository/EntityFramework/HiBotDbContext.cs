using System;
using System.Data.Entity;
using System.Linq;
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
            // the terrible hack
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>().ToTable("Students");
            modelBuilder.Entity<Teachers>().ToTable("Teachers");
            modelBuilder.Entity<CollegeStudent>().ToTable("CollegeStudent");
            modelBuilder.Entity<HistoryMissUnderstand>().ToTable("HistoryMissUnderstand");


            base.OnModelCreating(modelBuilder);


        }

        public DbSet<HistoryMissUnderstand> HistoryMissUnderstand { get; set; }

        public DbSet<Students> Students { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<CollegeStudent> CollegeStudent { get; set; }

        public void RollBack()
        {
           
            var changedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
