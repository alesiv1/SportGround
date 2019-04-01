namespace SportGround.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableCourtWorkingHours : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtEntities", t => t.CourtId_Id)
                .Index(t => t.CourtId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourtWorkingHoursEntities", "CourtId_Id", "dbo.CourtEntities");
            DropIndex("dbo.CourtWorkingHoursEntities", new[] { "CourtId_Id" });
            DropTable("dbo.CourtWorkingHoursEntities");
        }
    }
}
