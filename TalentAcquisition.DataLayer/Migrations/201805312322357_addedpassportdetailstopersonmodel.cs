namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpassportdetailstopersonmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "PassportDetails", c => c.String());
            AddColumn("dbo.JobSeeker", "PassportDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobSeeker", "PassportDetails");
            DropColumn("dbo.Employee", "PassportDetails");
        }
    }
}
