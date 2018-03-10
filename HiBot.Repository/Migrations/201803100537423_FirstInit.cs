namespace HiBot.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollegeStudent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentCode = c.String(),
                        Password = c.String(),
                        Money = c.String(),
                        Classmate = c.String(),
                        Name = c.String(),
                        Birthday = c.String(),
                        Sex = c.Boolean(nullable: false),
                        PhoneNumber = c.String(),
                        LastOnline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HighSchool = c.String(),
                        Name = c.String(),
                        Birthday = c.String(),
                        Sex = c.Boolean(nullable: false),
                        PhoneNumber = c.String(),
                        LastOnline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherCode = c.String(),
                        Name = c.String(),
                        Birthday = c.String(),
                        Sex = c.Boolean(nullable: false),
                        PhoneNumber = c.String(),
                        LastOnline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
            DropTable("dbo.CollegeStudent");
        }
    }
}
