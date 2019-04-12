namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataBase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourtWorkingHoursEntities", "CourtId_Id", "dbo.CourtEntities");
            DropIndex("dbo.CourtWorkingHoursEntities", new[] { "CourtId_Id" });
            CreateTable(
                "dbo.CourtWorkingDaysEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        StartTimeOfDay = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTimeOfDay = c.DateTimeOffset(nullable: false, precision: 7),
                        Court_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtEntities", t => t.Court_Id)
                .Index(t => t.Court_Id);
            
            AddColumn("dbo.CourtBookingEntities", "BookingDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.CourtBookingEntities", "Court_Id", c => c.Int());
            CreateIndex("dbo.CourtBookingEntities", "Court_Id");
            AddForeignKey("dbo.CourtBookingEntities", "Court_Id", "dbo.CourtEntities", "Id");
            DropColumn("dbo.CourtBookingEntities", "CourtId");
            DropColumn("dbo.CourtBookingEntities", "Date");
            DropTable("dbo.CourtWorkingHoursEntities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CourtWorkingHoursEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(nullable: false, precision: 7),
                        CourtId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CourtBookingEntities", "Date", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.CourtBookingEntities", "CourtId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CourtWorkingDaysEntities", "Court_Id", "dbo.CourtEntities");
            DropForeignKey("dbo.CourtBookingEntities", "Court_Id", "dbo.CourtEntities");
            DropIndex("dbo.CourtWorkingDaysEntities", new[] { "Court_Id" });
            DropIndex("dbo.CourtBookingEntities", new[] { "Court_Id" });
            DropColumn("dbo.CourtBookingEntities", "Court_Id");
            DropColumn("dbo.CourtBookingEntities", "BookingDate");
            DropTable("dbo.CourtWorkingDaysEntities");
            CreateIndex("dbo.CourtWorkingHoursEntities", "CourtId_Id");
            AddForeignKey("dbo.CourtWorkingHoursEntities", "CourtId_Id", "dbo.CourtEntities", "Id");
        }
    }
}
