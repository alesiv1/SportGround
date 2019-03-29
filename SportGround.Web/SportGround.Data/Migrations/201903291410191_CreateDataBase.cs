namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourtEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Role = c.Int(nullable: false),
                        Password = c.String(),
                        Salt = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserEntities");
            DropTable("dbo.CourtEntities");
        }
    }
}
