namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedGrievanceReportModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrievanceReport", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrievanceReport", "IsClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrievanceReport", "IsClosed");
            DropColumn("dbo.GrievanceReport", "IsApproved");
        }
    }
}
