namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModelsForOnboarding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompletedActivity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OnboardActivityID = c.Int(nullable: false),
                        HasTaskBeenCompleted = c.Boolean(),
                        WelcomeGuideID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OnboardActivity", t => t.OnboardActivityID, cascadeDelete: true)
                .ForeignKey("dbo.WelcomeGuide", t => t.WelcomeGuideID, cascadeDelete: true)
                .Index(t => t.OnboardActivityID)
                .Index(t => t.WelcomeGuideID);
            
            CreateTable(
                "dbo.OnboardActivity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        WelcomeGuide_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WelcomeGuide", t => t.WelcomeGuide_ID)
                .Index(t => t.WelcomeGuide_ID);
            
            CreateTable(
                "dbo.OnboardingTemplate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        WelcomeMessage = c.String(),
                        Location = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.WelcomeGuide",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        WelcomeMessage = c.String(),
                        Location = c.String(),
                        BranchID = c.Int(),
                        JobSeekerID = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Branch_BranchId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branch", t => t.Branch_BranchId)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeekerID)
                .Index(t => t.JobSeekerID)
                .Index(t => t.Branch_BranchId);
            
            CreateTable(
                "dbo.OnboardingTemplateOnboardActivity",
                c => new
                    {
                        OnboardingTemplate_ID = c.Int(nullable: false),
                        OnboardActivity_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OnboardingTemplate_ID, t.OnboardActivity_ID })
                .ForeignKey("dbo.OnboardingTemplate", t => t.OnboardingTemplate_ID, cascadeDelete: true)
                .ForeignKey("dbo.OnboardActivity", t => t.OnboardActivity_ID, cascadeDelete: true)
                .Index(t => t.OnboardingTemplate_ID)
                .Index(t => t.OnboardActivity_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OnboardActivity", "WelcomeGuide_ID", "dbo.WelcomeGuide");
            DropForeignKey("dbo.WelcomeGuide", "JobSeekerID", "dbo.JobSeeker");
            DropForeignKey("dbo.CompletedActivity", "WelcomeGuideID", "dbo.WelcomeGuide");
            DropForeignKey("dbo.WelcomeGuide", "Branch_BranchId", "dbo.Branch");
            DropForeignKey("dbo.CompletedActivity", "OnboardActivityID", "dbo.OnboardActivity");
            DropForeignKey("dbo.OnboardingTemplateOnboardActivity", "OnboardActivity_ID", "dbo.OnboardActivity");
            DropForeignKey("dbo.OnboardingTemplateOnboardActivity", "OnboardingTemplate_ID", "dbo.OnboardingTemplate");
            DropIndex("dbo.OnboardingTemplateOnboardActivity", new[] { "OnboardActivity_ID" });
            DropIndex("dbo.OnboardingTemplateOnboardActivity", new[] { "OnboardingTemplate_ID" });
            DropIndex("dbo.WelcomeGuide", new[] { "Branch_BranchId" });
            DropIndex("dbo.WelcomeGuide", new[] { "JobSeekerID" });
            DropIndex("dbo.OnboardActivity", new[] { "WelcomeGuide_ID" });
            DropIndex("dbo.CompletedActivity", new[] { "WelcomeGuideID" });
            DropIndex("dbo.CompletedActivity", new[] { "OnboardActivityID" });
            DropTable("dbo.OnboardingTemplateOnboardActivity");
            DropTable("dbo.WelcomeGuide");
            DropTable("dbo.OnboardingTemplate");
            DropTable("dbo.OnboardActivity");
            DropTable("dbo.CompletedActivity");
        }
    }
}
