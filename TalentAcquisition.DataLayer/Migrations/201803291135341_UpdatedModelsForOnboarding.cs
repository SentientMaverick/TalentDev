namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModelsForOnboarding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WelcomeGuide", "Name", c => c.String(nullable: false));
            AddColumn("dbo.WelcomeGuide", "Position", c => c.String());
            AlterColumn("dbo.OnboardingTemplate", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.OnboardingTemplate", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.OnboardingTemplate", "WelcomeMessage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OnboardingTemplate", "WelcomeMessage", c => c.String());
            AlterColumn("dbo.OnboardingTemplate", "Description", c => c.String());
            AlterColumn("dbo.OnboardingTemplate", "Title", c => c.String());
            DropColumn("dbo.WelcomeGuide", "Position");
            DropColumn("dbo.WelcomeGuide", "Name");
        }
    }
}
