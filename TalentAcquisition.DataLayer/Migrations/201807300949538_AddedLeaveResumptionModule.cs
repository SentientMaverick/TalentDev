namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLeaveResumptionModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeaveResumption",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LeaveAppID = c.Int(nullable: false),
                        EmployeeId = c.String(),
                        EmployeeName = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        ResumptionDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LeaveResumption");
        }
    }
}
