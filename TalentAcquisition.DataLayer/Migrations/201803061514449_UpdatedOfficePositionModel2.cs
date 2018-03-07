namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedOfficePositionModel2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfficePosition", "JobID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfficePosition", "JobID");
        }
    }
}
