namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateJobseekerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobSeeker", "UploadedPassportAddress", c => c.String());
            AddColumn("dbo.JobSeeker", "IndustryID", c => c.String());
            AddColumn("dbo.JobSeeker", "Industry_IndustryId", c => c.Int());
            CreateIndex("dbo.JobSeeker", "Industry_IndustryId");
            AddForeignKey("dbo.JobSeeker", "Industry_IndustryId", "dbo.Industry", "IndustryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobSeeker", "Industry_IndustryId", "dbo.Industry");
            DropIndex("dbo.JobSeeker", new[] { "Industry_IndustryId" });
            DropColumn("dbo.JobSeeker", "Industry_IndustryId");
            DropColumn("dbo.JobSeeker", "IndustryID");
            DropColumn("dbo.JobSeeker", "UploadedPassportAddress");
        }
    }
}
