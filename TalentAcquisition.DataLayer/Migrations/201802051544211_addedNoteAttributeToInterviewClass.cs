namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedNoteAttributeToInterviewClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interview", "SchedulingFinalNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interview", "SchedulingFinalNote");
        }
    }
}
