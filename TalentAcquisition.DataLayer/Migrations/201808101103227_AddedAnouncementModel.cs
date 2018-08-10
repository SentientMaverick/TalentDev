namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAnouncementModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeID = c.String(),
                        EmployeeName = c.String(),
                        Title = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastUpdatedAt = c.DateTime(),
                        Employee_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.Employee_ID)
                .Index(t => t.Employee_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Announcement", "Employee_ID", "dbo.Employee");
            DropIndex("dbo.Announcement", new[] { "Employee_ID" });
            DropTable("dbo.Announcement");
        }
    }
}
