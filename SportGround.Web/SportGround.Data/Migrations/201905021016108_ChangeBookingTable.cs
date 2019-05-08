namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBookingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtBookingEntities", "StartDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.CourtBookingEntities", "EndDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.CourtBookingEntities", "BookingDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtBookingEntities", "BookingDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.CourtBookingEntities", "EndDate");
            DropColumn("dbo.CourtBookingEntities", "StartDate");
        }
    }
}
