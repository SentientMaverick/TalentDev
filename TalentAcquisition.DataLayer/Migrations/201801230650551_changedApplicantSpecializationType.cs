namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedApplicantSpecializationType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobSeeker", new[] { "Industry_IndustryId" });
            DropColumn("dbo.JobSeeker", "IndustryID");
            RenameColumn(table: "dbo.JobSeeker", name: "Industry_IndustryId", newName: "IndustryID");
            AlterColumn("dbo.JobSeeker", "IndustryID", c => c.Int());
            CreateIndex("dbo.JobSeeker", "IndustryID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobSeeker", new[] { "IndustryID" });
            AlterColumn("dbo.JobSeeker", "IndustryID", c => c.String());
            RenameColumn(table: "dbo.JobSeeker", name: "IndustryID", newName: "Industry_IndustryId");
            AddColumn("dbo.JobSeeker", "IndustryID", c => c.String());
            CreateIndex("dbo.JobSeeker", "Industry_IndustryId");
        }
    }
}
