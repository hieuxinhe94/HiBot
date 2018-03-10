using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using HiBot.Entities;

namespace HiBot.Repository.EntityFramework.Mapping
{
  public class StudentMap : EntityTypeConfiguration<Students>
    {
        // Khong can lam cach nay lam
        public StudentMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            ToTable("Students");
        }
       
    }
}
