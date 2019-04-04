namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTableForCourtBooking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourtBookingEntities", "Court_Id", "dbo.CourtEntities");
            DropIndex("dbo.CourtBookingEntities", new[] { "Court_Id" });
            AddColumn("dbo.CourtBookingEntities", "CourtId", c => c.Int(nullable: false));
            DropColumn("dbo.CourtBookingEntities", "Court_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtBookingEntities", "Court_Id", c => c.Int());
            DropColumn("dbo.CourtBookingEntities", "CourtId");
            CreateIndex("dbo.CourtBookingEntities", "Court_Id");
            AddForeignKey("dbo.CourtBookingEntities", "Court_Id", "dbo.CourtEntities", "Id");
        }
    }
}
