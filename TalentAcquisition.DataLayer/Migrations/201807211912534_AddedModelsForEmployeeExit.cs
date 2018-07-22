namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModelsForEmployeeExit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExitActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExitActivityLine",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        ExitInterviewNo = c.String(maxLength: 128),
                        Completed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExitInterview", t => t.ExitInterviewNo)
                .Index(t => t.ExitInterviewNo);
            
            CreateTable(
                "dbo.ExitInterview",
                c => new
                    {
                        No = c.String(nullable: false, maxLength: 128),
                        EmployeeNo = c.String(nullable: false),
                        EmployeeName = c.String(nullable: false),
                        Reason = c.String(nullable: false),
                        OtherReasons = c.String(),
                        LeavingOn = c.DateTime(nullable: false),
                        ReEmploy = c.Boolean(nullable: false),
                        InterviewDate = c.DateTime(nullable: false),
                        InterviewerNo = c.String(),
                        InterviewerName = c.String(),
                        Comment = c.String(),
                        Status = c.String(),
                        ApproveProcess = c.Boolean(nullable: false),
                        ProcessCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.No);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExitActivityLine", "ExitInterviewNo", "dbo.ExitInterview");
            DropIndex("dbo.ExitActivityLine", new[] { "ExitInterviewNo" });
            DropTable("dbo.ExitInterview");
            DropTable("dbo.ExitActivityLine");
            DropTable("dbo.ExitActivity");
        }
    }
}
