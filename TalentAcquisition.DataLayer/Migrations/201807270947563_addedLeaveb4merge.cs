namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLeaveb4merge : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeaveApplication",
                c => new
                    {
                        LeaveAppID = c.Int(nullable: false, identity: true),
                        LeavePlanID = c.Int(),
                        EmployeeId = c.String(),
                        EmployeeName = c.String(),
                        LeaveType = c.String(),
                        LeaveLimit = c.Int(),
                        TotalLeaveTaken = c.Int(),
                        TotalLeaveAvailable = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        ApplyDate = c.DateTime(nullable: false),
                        LeavePlanStatus = c.String(),
                        LeaveAppStatus = c.Int(nullable: false),
                        LeaveType_Limit_ID = c.Int(),
                    })
                .PrimaryKey(t => t.LeaveAppID)
                .ForeignKey("dbo.LeaveType_Limit", t => t.LeaveType_Limit_ID)
                .Index(t => t.LeaveType_Limit_ID);
            
            CreateTable(
                "dbo.LeaveType_Limit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LeaveType = c.String(),
                        Limit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ManageEmployeeLeave",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(),
                        EmployeeName = c.String(),
                        LeaveType = c.String(),
                        LeaveLimit = c.Int(),
                        TotalLeaveTaken = c.Int(),
                        TotalLeaveAvailable = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        ApplyDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LeaveType_Limit_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LeaveType_Limit", t => t.LeaveType_Limit_ID)
                .Index(t => t.LeaveType_Limit_ID);
            
            AddColumn("dbo.Employee", "OfficialEmail", c => c.String());
            AddColumn("dbo.Employee", "ContactEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveApplication", "LeaveType_Limit_ID", "dbo.LeaveType_Limit");
            DropForeignKey("dbo.ManageEmployeeLeave", "LeaveType_Limit_ID", "dbo.LeaveType_Limit");
            DropIndex("dbo.ManageEmployeeLeave", new[] { "LeaveType_Limit_ID" });
            DropIndex("dbo.LeaveApplication", new[] { "LeaveType_Limit_ID" });
            DropColumn("dbo.Employee", "ContactEmail");
            DropColumn("dbo.Employee", "OfficialEmail");
            DropTable("dbo.ManageEmployeeLeave");
            DropTable("dbo.LeaveType_Limit");
            DropTable("dbo.LeaveApplication");
        }
    }
}
