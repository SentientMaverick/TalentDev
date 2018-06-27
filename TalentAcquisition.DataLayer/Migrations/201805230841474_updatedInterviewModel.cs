namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedInterviewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interview", "HasInterviewBeenCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Interview", "StageID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interview", "StageID");
            DropColumn("dbo.Interview", "HasInterviewBeenCompleted");
        }
    }
}
