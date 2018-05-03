namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEvaluationCategoryAndInterviewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interview", "Venue", c => c.String());
            AddColumn("dbo.EvaluationCategory", "OfficePositionID", c => c.Int());
            CreateIndex("dbo.EvaluationCategory", "OfficePositionID");
            AddForeignKey("dbo.EvaluationCategory", "OfficePositionID", "dbo.OfficePosition", "OfficePositionID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EvaluationCategory", "OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.EvaluationCategory", new[] { "OfficePositionID" });
            DropColumn("dbo.EvaluationCategory", "OfficePositionID");
            DropColumn("dbo.Interview", "Venue");
        }
    }
}
