namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedOfficePositionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobOccupant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeNumber = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        OfficePosition_OfficePositionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OfficePosition", t => t.OfficePosition_OfficePositionID)
                .Index(t => t.OfficePosition_OfficePositionID);
            
            CreateTable(
                "dbo.JobQualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QualificationType = c.String(),
                        QualificationCode = c.String(),
                        OfficePosition_OfficePositionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OfficePosition", t => t.OfficePosition_OfficePositionID)
                .Index(t => t.OfficePosition_OfficePositionID);
            
            CreateTable(
                "dbo.JobRequirement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QualificationType = c.String(),
                        QualificationCode = c.String(),
                        QualificationDescription = c.String(),
                        Priority = c.String(),
                        ScoreID = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NeedCode = c.Double(nullable: false),
                        StageCode = c.Double(nullable: false),
                        Mandatory = c.Boolean(nullable: false),
                        DesiredScore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OfficePosition_OfficePositionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OfficePosition", t => t.OfficePosition_OfficePositionID)
                .Index(t => t.OfficePosition_OfficePositionID);
            
            AddColumn("dbo.OfficePosition", "JobDescription", c => c.String());
            AddColumn("dbo.OfficePosition", "Posts", c => c.Int(nullable: false));
            AddColumn("dbo.OfficePosition", "SupervisorID", c => c.Int());
            AddColumn("dbo.OfficePosition", "BranchID", c => c.Int());
            AddColumn("dbo.OfficePosition", "MainObjective", c => c.String());
            AddColumn("dbo.OfficePosition", "Grade", c => c.String());
            AddColumn("dbo.OfficePosition", "RequisitionCount", c => c.Int(nullable: false));
            AddColumn("dbo.OfficePosition", "Status", c => c.String());
            AddColumn("dbo.OfficePosition", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobRequirement", "OfficePosition_OfficePositionID", "dbo.OfficePosition");
            DropForeignKey("dbo.JobQualification", "OfficePosition_OfficePositionID", "dbo.OfficePosition");
            DropForeignKey("dbo.JobOccupant", "OfficePosition_OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.JobRequirement", new[] { "OfficePosition_OfficePositionID" });
            DropIndex("dbo.JobQualification", new[] { "OfficePosition_OfficePositionID" });
            DropIndex("dbo.JobOccupant", new[] { "OfficePosition_OfficePositionID" });
            DropColumn("dbo.OfficePosition", "DateCreated");
            DropColumn("dbo.OfficePosition", "Status");
            DropColumn("dbo.OfficePosition", "RequisitionCount");
            DropColumn("dbo.OfficePosition", "Grade");
            DropColumn("dbo.OfficePosition", "MainObjective");
            DropColumn("dbo.OfficePosition", "BranchID");
            DropColumn("dbo.OfficePosition", "SupervisorID");
            DropColumn("dbo.OfficePosition", "Posts");
            DropColumn("dbo.OfficePosition", "JobDescription");
            DropTable("dbo.JobRequirement");
            DropTable("dbo.JobQualification");
            DropTable("dbo.JobOccupant");
        }
    }
}
