namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedWelcomeGuideModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WelcomeGuide", "previewurl", c => c.String());
            AddColumn("dbo.WelcomeGuide", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.WelcomeGuide", "TemplateID", c => c.Int());
            AlterColumn("dbo.WelcomeGuide", "Position", c => c.String(nullable: false));
            DropColumn("dbo.WelcomeGuide", "Title");
            DropColumn("dbo.WelcomeGuide", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WelcomeGuide", "Description", c => c.String());
            AddColumn("dbo.WelcomeGuide", "Title", c => c.String());
            AlterColumn("dbo.WelcomeGuide", "Position", c => c.String());
            DropColumn("dbo.WelcomeGuide", "TemplateID");
            DropColumn("dbo.WelcomeGuide", "Status");
            DropColumn("dbo.WelcomeGuide", "previewurl");
        }
    }
}
