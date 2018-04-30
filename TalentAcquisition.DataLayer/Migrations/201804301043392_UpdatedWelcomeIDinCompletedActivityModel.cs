namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedWelcomeIDinCompletedActivityModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompletedActivity", "WelcomeGuideID", "dbo.WelcomeGuide");
            DropIndex("dbo.CompletedActivity", new[] { "WelcomeGuideID" });
            AlterColumn("dbo.CompletedActivity", "WelcomeGuideID", c => c.Int());
            CreateIndex("dbo.CompletedActivity", "WelcomeGuideID");
            AddForeignKey("dbo.CompletedActivity", "WelcomeGuideID", "dbo.WelcomeGuide", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompletedActivity", "WelcomeGuideID", "dbo.WelcomeGuide");
            DropIndex("dbo.CompletedActivity", new[] { "WelcomeGuideID" });
            AlterColumn("dbo.CompletedActivity", "WelcomeGuideID", c => c.Int(nullable: false));
            CreateIndex("dbo.CompletedActivity", "WelcomeGuideID");
            AddForeignKey("dbo.CompletedActivity", "WelcomeGuideID", "dbo.WelcomeGuide", "ID", cascadeDelete: true);
        }
    }
}
