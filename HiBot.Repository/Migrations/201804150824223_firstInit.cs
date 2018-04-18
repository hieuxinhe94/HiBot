namespace HiBot.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistoryMissUnderstand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sentences = c.String(),
                        Intent = c.String(),
                        Reply = c.String(),
                        TimeZone = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Students", "Khoi", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "DiemExpect", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "DiemExpect");
            DropColumn("dbo.Students", "Khoi");
            DropTable("dbo.HistoryMissUnderstand");
        }
    }
}
