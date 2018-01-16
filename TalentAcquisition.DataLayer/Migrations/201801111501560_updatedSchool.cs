namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedSchool : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.School", "JobSeeker_ID", "dbo.JobSeeker");
            DropForeignKey("dbo.WorkExperience", "JobSeeker_ID", "dbo.JobSeeker");
            DropIndex("dbo.School", new[] { "JobSeeker_ID" });
            DropIndex("dbo.WorkExperience", new[] { "JobSeeker_ID" });
            RenameColumn(table: "dbo.School", name: "JobSeeker_ID", newName: "JobSeekerID");
            RenameColumn(table: "dbo.WorkExperience", name: "JobSeeker_ID", newName: "JobSeekerID");
            AddColumn("dbo.School", "SchoolName", c => c.String());
            AddColumn("dbo.School", "Level", c => c.Int(nullable: false));
            AddColumn("dbo.School", "CourseOfStudy", c => c.String());
            AddColumn("dbo.School", "StartingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.School", "EndingDate", c => c.DateTime());
            AddColumn("dbo.WorkExperience", "CompanyName", c => c.String());
            AddColumn("dbo.WorkExperience", "JobTitle", c => c.String());
            AddColumn("dbo.WorkExperience", "JobDescription", c => c.String());
            AddColumn("dbo.WorkExperience", "ReasonsForLeaving", c => c.String());
            AddColumn("dbo.WorkExperience", "StartingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkExperience", "EndingDate", c => c.DateTime());
            AlterColumn("dbo.School", "JobSeekerID", c => c.Int(nullable: false));
            AlterColumn("dbo.WorkExperience", "JobSeekerID", c => c.Int(nullable: false));
            CreateIndex("dbo.School", "JobSeekerID");
            CreateIndex("dbo.WorkExperience", "JobSeekerID");
            AddForeignKey("dbo.School", "JobSeekerID", "dbo.JobSeeker", "ID", cascadeDelete: true);
            AddForeignKey("dbo.WorkExperience", "JobSeekerID", "dbo.JobSeeker", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkExperience", "JobSeekerID", "dbo.JobSeeker");
            DropForeignKey("dbo.School", "JobSeekerID", "dbo.JobSeeker");
            DropIndex("dbo.WorkExperience", new[] { "JobSeekerID" });
            DropIndex("dbo.School", new[] { "JobSeekerID" });
            AlterColumn("dbo.WorkExperience", "JobSeekerID", c => c.Int());
            AlterColumn("dbo.School", "JobSeekerID", c => c.Int());
            DropColumn("dbo.WorkExperience", "EndingDate");
            DropColumn("dbo.WorkExperience", "StartingDate");
            DropColumn("dbo.WorkExperience", "ReasonsForLeaving");
            DropColumn("dbo.WorkExperience", "JobDescription");
            DropColumn("dbo.WorkExperience", "JobTitle");
            DropColumn("dbo.WorkExperience", "CompanyName");
            DropColumn("dbo.School", "EndingDate");
            DropColumn("dbo.School", "StartingDate");
            DropColumn("dbo.School", "CourseOfStudy");
            DropColumn("dbo.School", "Level");
            DropColumn("dbo.School", "SchoolName");
            RenameColumn(table: "dbo.WorkExperience", name: "JobSeekerID", newName: "JobSeeker_ID");
            RenameColumn(table: "dbo.School", name: "JobSeekerID", newName: "JobSeeker_ID");
            CreateIndex("dbo.WorkExperience", "JobSeeker_ID");
            CreateIndex("dbo.School", "JobSeeker_ID");
            AddForeignKey("dbo.WorkExperience", "JobSeeker_ID", "dbo.JobSeeker", "ID");
            AddForeignKey("dbo.School", "JobSeeker_ID", "dbo.JobSeeker", "ID");
        }
    }
}
