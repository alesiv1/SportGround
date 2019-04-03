namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourtBookingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourtBookingEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Court_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtEntities", t => t.Court_Id)
                .ForeignKey("dbo.UserEntities", t => t.User_Id)
                .Index(t => t.Court_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourtBookingEntities", "User_Id", "dbo.UserEntities");
            DropForeignKey("dbo.CourtBookingEntities", "Court_Id", "dbo.CourtEntities");
            DropIndex("dbo.CourtBookingEntities", new[] { "User_Id" });
            DropIndex("dbo.CourtBookingEntities", new[] { "Court_Id" });
            DropTable("dbo.CourtBookingEntities");
        }
    }
}
