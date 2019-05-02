namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdInLongTimeForBooking : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CourtBookingEntities");
            AlterColumn("dbo.CourtBookingEntities", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.CourtBookingEntities", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CourtBookingEntities");
            AlterColumn("dbo.CourtBookingEntities", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CourtBookingEntities", "Id");
        }
    }
}
