namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCompletedActivityModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompletedActivity", "Name", c => c.String());
            AddColumn("dbo.CompletedActivity", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompletedActivity", "Description");
            DropColumn("dbo.CompletedActivity", "Name");
        }
    }
}
