namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobRequisition", "HumanResourcePersonnelID", "dbo.Employee");
            DropIndex("dbo.JobRequisition", new[] { "HumanResourcePersonnelID" });
            CreateTable(
                "dbo.Industry",
                c => new
                    {
                        IndustryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.IndustryId);
            
            AddColumn("dbo.JobSeeker", "UploadedCVAddress", c => c.String());
            AddColumn("dbo.JobRequisition", "Status", c => c.Int());
            AddColumn("dbo.JobRequisition", "NoOfPositionsAvailable", c => c.Int(nullable: false));
            AddColumn("dbo.JobRequisition", "HeadOfDepartmentID", c => c.Int(nullable: false));
            AddColumn("dbo.JobRequisition", "JobUrl", c => c.String());
            AddColumn("dbo.JobRequisition", "Employee_ID", c => c.Int());
            AddColumn("dbo.JobRequisition", "Employee_ID1", c => c.Int());
            AddColumn("dbo.JobRequisition", "Employee1_ID", c => c.Int());
            AddColumn("dbo.OfficePosition", "IndustryID", c => c.Int(nullable: false));
            CreateIndex("dbo.JobRequisition", "Employee_ID");
            CreateIndex("dbo.JobRequisition", "Employee_ID1");
            CreateIndex("dbo.JobRequisition", "Employee1_ID");
            CreateIndex("dbo.OfficePosition", "IndustryID");
            AddForeignKey("dbo.JobRequisition", "Employee_ID", "dbo.Employee", "ID");
            AddForeignKey("dbo.OfficePosition", "IndustryID", "dbo.Industry", "IndustryId", cascadeDelete: true);
            AddForeignKey("dbo.JobRequisition", "Employee1_ID", "dbo.Employee", "ID");
            AddForeignKey("dbo.JobRequisition", "Employee_ID1", "dbo.Employee", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobRequisition", "Employee_ID1", "dbo.Employee");
            DropForeignKey("dbo.JobRequisition", "Employee1_ID", "dbo.Employee");
            DropForeignKey("dbo.OfficePosition", "IndustryID", "dbo.Industry");
            DropForeignKey("dbo.JobRequisition", "Employee_ID", "dbo.Employee");
            DropIndex("dbo.OfficePosition", new[] { "IndustryID" });
            DropIndex("dbo.JobRequisition", new[] { "Employee1_ID" });
            DropIndex("dbo.JobRequisition", new[] { "Employee_ID1" });
            DropIndex("dbo.JobRequisition", new[] { "Employee_ID" });
            DropColumn("dbo.OfficePosition", "IndustryID");
            DropColumn("dbo.JobRequisition", "Employee1_ID");
            DropColumn("dbo.JobRequisition", "Employee_ID1");
            DropColumn("dbo.JobRequisition", "Employee_ID");
            DropColumn("dbo.JobRequisition", "JobUrl");
            DropColumn("dbo.JobRequisition", "HeadOfDepartmentID");
            DropColumn("dbo.JobRequisition", "NoOfPositionsAvailable");
            DropColumn("dbo.JobRequisition", "Status");
            DropColumn("dbo.JobSeeker", "UploadedCVAddress");
            DropTable("dbo.Industry");
            CreateIndex("dbo.JobRequisition", "HumanResourcePersonnelID");
            AddForeignKey("dbo.JobRequisition", "HumanResourcePersonnelID", "dbo.Employee", "ID", cascadeDelete: true);
        }
    }
}
