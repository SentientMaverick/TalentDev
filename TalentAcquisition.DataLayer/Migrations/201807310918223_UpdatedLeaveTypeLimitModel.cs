namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedLeaveTypeLimitModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveType_Limit", "RequiresPlan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveType_Limit", "RequiresPlan");
        }
    }
}
