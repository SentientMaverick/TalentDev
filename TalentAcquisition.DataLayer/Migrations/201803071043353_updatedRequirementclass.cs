namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedRequirementclass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobRequirement", "OfficePosition_OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.JobRequirement", new[] { "OfficePosition_OfficePositionID" });
            RenameColumn(table: "dbo.JobRequirement", name: "OfficePosition_OfficePositionID", newName: "OfficePositionID");
            AlterColumn("dbo.JobRequirement", "OfficePositionID", c => c.Int(nullable: false));
            CreateIndex("dbo.JobRequirement", "OfficePositionID");
            AddForeignKey("dbo.JobRequirement", "OfficePositionID", "dbo.OfficePosition", "OfficePositionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobRequirement", "OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.JobRequirement", new[] { "OfficePositionID" });
            AlterColumn("dbo.JobRequirement", "OfficePositionID", c => c.Int());
            RenameColumn(table: "dbo.JobRequirement", name: "OfficePositionID", newName: "OfficePosition_OfficePositionID");
            CreateIndex("dbo.JobRequirement", "OfficePosition_OfficePositionID");
            AddForeignKey("dbo.JobRequirement", "OfficePosition_OfficePositionID", "dbo.OfficePosition", "OfficePositionID");
        }
    }
}
