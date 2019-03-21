namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRegistrationTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Id", "dbo.Registration");
            DropIndex("dbo.Users", new[] { "Id" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "Id");
            DropTable("dbo.Registration");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Registration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 255),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Users", "Id");
            CreateIndex("dbo.Users", "Id");
            AddForeignKey("dbo.Users", "Id", "dbo.Registration", "Id");
        }
    }
}
