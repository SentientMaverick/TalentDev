namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldsToLeaveResumptionModule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveResumption", "LeaveType", c => c.String());
            AddColumn("dbo.LeaveResumption", "ApplyDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveResumption", "ApplyDate");
            DropColumn("dbo.LeaveResumption", "LeaveType");
        }
    }
}
