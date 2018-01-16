namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedCertificationModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Certification", "JobSeeker_ID", "dbo.JobSeeker");
            DropIndex("dbo.Certification", new[] { "JobSeeker_ID" });
            RenameColumn(table: "dbo.Certification", name: "JobSeeker_ID", newName: "JobSeekerID");
            AddColumn("dbo.Certification", "Title", c => c.String(nullable: false));
            AddColumn("dbo.Certification", "Year", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Certification", "JobSeekerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Certification", "JobSeekerID");
            AddForeignKey("dbo.Certification", "JobSeekerID", "dbo.JobSeeker", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Certification", "JobSeekerID", "dbo.JobSeeker");
            DropIndex("dbo.Certification", new[] { "JobSeekerID" });
            AlterColumn("dbo.Certification", "JobSeekerID", c => c.Int());
            DropColumn("dbo.Certification", "Year");
            DropColumn("dbo.Certification", "Title");
            RenameColumn(table: "dbo.Certification", name: "JobSeekerID", newName: "JobSeeker_ID");
            CreateIndex("dbo.Certification", "JobSeeker_ID");
            AddForeignKey("dbo.Certification", "JobSeeker_ID", "dbo.JobSeeker", "ID");
        }
    }
}
