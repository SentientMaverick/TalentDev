namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedOnboardingRelatedModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OnboardingTemplateOnboardActivity", "OnboardingTemplate_ID", "dbo.OnboardingTemplate");
            DropForeignKey("dbo.OnboardingTemplateOnboardActivity", "OnboardActivity_ID", "dbo.OnboardActivity");
            DropForeignKey("dbo.CompletedActivity", "OnboardActivityID", "dbo.OnboardActivity");
            DropForeignKey("dbo.OnboardActivity", "WelcomeGuide_ID", "dbo.WelcomeGuide");
            DropIndex("dbo.CompletedActivity", new[] { "OnboardActivityID" });
            DropIndex("dbo.OnboardActivity", new[] { "WelcomeGuide_ID" });
            DropIndex("dbo.OnboardingTemplateOnboardActivity", new[] { "OnboardingTemplate_ID" });
            DropIndex("dbo.OnboardingTemplateOnboardActivity", new[] { "OnboardActivity_ID" });
            AddColumn("dbo.CompletedActivity", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.CompletedActivity", "OnboardingTemplateID", c => c.Int(nullable: false));
            AddColumn("dbo.CompletedActivity", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OnboardActivity", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.CompletedActivity", "HasTaskBeenCompleted", c => c.Boolean(nullable: false));
            CreateIndex("dbo.CompletedActivity", "OnboardingTemplateID");
            AddForeignKey("dbo.CompletedActivity", "OnboardingTemplateID", "dbo.OnboardingTemplate", "ID", cascadeDelete: true);
            DropColumn("dbo.OnboardActivity", "WelcomeGuide_ID");
            DropTable("dbo.OnboardingTemplateOnboardActivity");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OnboardingTemplateOnboardActivity",
                c => new
                    {
                        OnboardingTemplate_ID = c.Int(nullable: false),
                        OnboardActivity_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OnboardingTemplate_ID, t.OnboardActivity_ID });
            
            AddColumn("dbo.OnboardActivity", "WelcomeGuide_ID", c => c.Int());
            DropForeignKey("dbo.CompletedActivity", "OnboardingTemplateID", "dbo.OnboardingTemplate");
            DropIndex("dbo.CompletedActivity", new[] { "OnboardingTemplateID" });
            AlterColumn("dbo.CompletedActivity", "HasTaskBeenCompleted", c => c.Boolean());
            DropColumn("dbo.OnboardActivity", "Type");
            DropColumn("dbo.CompletedActivity", "DueDate");
            DropColumn("dbo.CompletedActivity", "OnboardingTemplateID");
            DropColumn("dbo.CompletedActivity", "Type");
            CreateIndex("dbo.OnboardingTemplateOnboardActivity", "OnboardActivity_ID");
            CreateIndex("dbo.OnboardingTemplateOnboardActivity", "OnboardingTemplate_ID");
            CreateIndex("dbo.OnboardActivity", "WelcomeGuide_ID");
            CreateIndex("dbo.CompletedActivity", "OnboardActivityID");
            AddForeignKey("dbo.OnboardActivity", "WelcomeGuide_ID", "dbo.WelcomeGuide", "ID");
            AddForeignKey("dbo.CompletedActivity", "OnboardActivityID", "dbo.OnboardActivity", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OnboardingTemplateOnboardActivity", "OnboardActivity_ID", "dbo.OnboardActivity", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OnboardingTemplateOnboardActivity", "OnboardingTemplate_ID", "dbo.OnboardingTemplate", "ID", cascadeDelete: true);
        }
    }
}
