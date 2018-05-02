namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCompletedActivityModel1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompletedActivity", "OnboardingTemplateID", "dbo.OnboardingTemplate");
            DropIndex("dbo.CompletedActivity", new[] { "OnboardingTemplateID" });
            AlterColumn("dbo.CompletedActivity", "OnboardingTemplateID", c => c.Int());
            CreateIndex("dbo.CompletedActivity", "OnboardingTemplateID");
            AddForeignKey("dbo.CompletedActivity", "OnboardingTemplateID", "dbo.OnboardingTemplate", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompletedActivity", "OnboardingTemplateID", "dbo.OnboardingTemplate");
            DropIndex("dbo.CompletedActivity", new[] { "OnboardingTemplateID" });
            AlterColumn("dbo.CompletedActivity", "OnboardingTemplateID", c => c.Int(nullable: false));
            CreateIndex("dbo.CompletedActivity", "OnboardingTemplateID");
            AddForeignKey("dbo.CompletedActivity", "OnboardingTemplateID", "dbo.OnboardingTemplate", "ID", cascadeDelete: true);
        }
    }
}
