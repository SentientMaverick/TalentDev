namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedJobRequisitionModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobRequisition", "OfficePositionID", c => c.Int(nullable: false));
            AddColumn("dbo.JobRequisition", "Location", c => c.String());
            AddColumn("dbo.JobRequisition", "PublishedDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.JobRequisition", "OfficePositionID");
            AddForeignKey("dbo.JobRequisition", "OfficePositionID", "dbo.OfficePosition", "OfficePositionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobRequisition", "OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.JobRequisition", new[] { "OfficePositionID" });
            DropColumn("dbo.JobRequisition", "PublishedDate");
            DropColumn("dbo.JobRequisition", "Location");
            DropColumn("dbo.JobRequisition", "OfficePositionID");
        }
    }
}
