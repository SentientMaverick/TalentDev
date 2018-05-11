namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedInterviewModelandAppStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interview", "JobOfferMessage", c => c.String());
            AddColumn("dbo.Interview", "Time", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interview", "Time");
            DropColumn("dbo.Interview", "JobOfferMessage");
        }
    }
}
