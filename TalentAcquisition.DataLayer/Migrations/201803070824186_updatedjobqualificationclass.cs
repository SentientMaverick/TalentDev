namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedjobqualificationclass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobQualification", "OfficePosition_OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.JobQualification", new[] { "OfficePosition_OfficePositionID" });
            CreateTable(
                "dbo.Branch",
                c => new
                    {
                        BranchId = c.String(nullable: false, maxLength: 128),
                        Location = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BranchId);
            
            AddColumn("dbo.JobQualification", "Description", c => c.String());
            AddColumn("dbo.JobRequirement", "QualificationID", c => c.Int(nullable: false));
            DropColumn("dbo.JobQualification", "OfficePosition_OfficePositionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobQualification", "OfficePosition_OfficePositionID", c => c.Int());
            DropColumn("dbo.JobRequirement", "QualificationID");
            DropColumn("dbo.JobQualification", "Description");
            DropTable("dbo.Branch");
            CreateIndex("dbo.JobQualification", "OfficePosition_OfficePositionID");
            AddForeignKey("dbo.JobQualification", "OfficePosition_OfficePositionID", "dbo.OfficePosition", "OfficePositionID");
        }
    }
}
