namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedKeyToInterviewDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterviewDetail", "InterviewID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterviewDetail", "InterviewID");
        }
    }
}
