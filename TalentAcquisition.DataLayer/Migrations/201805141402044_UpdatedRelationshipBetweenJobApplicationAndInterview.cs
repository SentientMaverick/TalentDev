namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRelationshipBetweenJobApplicationAndInterview : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Interview", "JobApplicationID");
            AddForeignKey("dbo.Interview", "JobApplicationID", "dbo.JobApplication", "JobApplicationID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interview", "JobApplicationID", "dbo.JobApplication");
            DropIndex("dbo.Interview", new[] { "JobApplicationID" });
        }
    }
}
