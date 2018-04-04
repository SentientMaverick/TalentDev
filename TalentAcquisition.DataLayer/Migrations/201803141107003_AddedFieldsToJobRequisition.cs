namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldsToJobRequisition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobRequisition", "Employee_ID1", "dbo.Employee");
            DropForeignKey("dbo.JobRequisition", "Employee1_ID", "dbo.Employee");
            DropIndex("dbo.JobRequisition", new[] { "Employee_ID1" });
            DropIndex("dbo.JobRequisition", new[] { "Employee1_ID" });
            AddColumn("dbo.JobRequisition", "Priority", c => c.Int());
            AddColumn("dbo.JobRequisition", "Stage", c => c.String());
            AddColumn("dbo.JobRequisition", "Score", c => c.String());
            AddColumn("dbo.JobRequisition", "StageCode", c => c.String());
            AddColumn("dbo.JobRequisition", "RequisitionNo", c => c.String());
            AddColumn("dbo.JobRequisition", "GlobalDimension", c => c.String());
            AddColumn("dbo.JobRequisition", "Closed", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobRequisition", "Qualified", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobRequisition", "TurnAroundTime", c => c.Int());
            AddColumn("dbo.JobRequisition", "GracePeriod", c => c.Int());
            AddColumn("dbo.JobRequisition", "RequiredPositions", c => c.Int());
            AddColumn("dbo.JobRequisition", "VacantPositions", c => c.Int());
            AddColumn("dbo.JobRequisition", "RequisitionType", c => c.String());
            AddColumn("dbo.JobRequisition", "ReasonForRequest", c => c.String());
            AddColumn("dbo.JobRequisition", "AnyAdditionalInformation", c => c.String());
            AddColumn("dbo.JobRequisition", "JobGrade", c => c.String());
            AddColumn("dbo.JobRequisition", "TypeOfContractRequired", c => c.String());
            AddColumn("dbo.JobRequisition", "NoSeries", c => c.String());
            AddColumn("dbo.JobRequisition", "ResponsibilityCenter", c => c.String());
            DropColumn("dbo.JobRequisition", "joburl");
            DropColumn("dbo.JobRequisition", "Employee_ID1");
            DropColumn("dbo.JobRequisition", "Employee1_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobRequisition", "Employee1_ID", c => c.Int());
            AddColumn("dbo.JobRequisition", "Employee_ID1", c => c.Int());
            AddColumn("dbo.JobRequisition", "joburl", c => c.String());
            DropColumn("dbo.JobRequisition", "ResponsibilityCenter");
            DropColumn("dbo.JobRequisition", "NoSeries");
            DropColumn("dbo.JobRequisition", "TypeOfContractRequired");
            DropColumn("dbo.JobRequisition", "JobGrade");
            DropColumn("dbo.JobRequisition", "AnyAdditionalInformation");
            DropColumn("dbo.JobRequisition", "ReasonForRequest");
            DropColumn("dbo.JobRequisition", "RequisitionType");
            DropColumn("dbo.JobRequisition", "VacantPositions");
            DropColumn("dbo.JobRequisition", "RequiredPositions");
            DropColumn("dbo.JobRequisition", "GracePeriod");
            DropColumn("dbo.JobRequisition", "TurnAroundTime");
            DropColumn("dbo.JobRequisition", "Qualified");
            DropColumn("dbo.JobRequisition", "Closed");
            DropColumn("dbo.JobRequisition", "GlobalDimension");
            DropColumn("dbo.JobRequisition", "RequisitionNo");
            DropColumn("dbo.JobRequisition", "StageCode");
            DropColumn("dbo.JobRequisition", "Score");
            DropColumn("dbo.JobRequisition", "Stage");
            DropColumn("dbo.JobRequisition", "Priority");
            CreateIndex("dbo.JobRequisition", "Employee1_ID");
            CreateIndex("dbo.JobRequisition", "Employee_ID1");
            AddForeignKey("dbo.JobRequisition", "Employee1_ID", "dbo.Employee", "ID");
            AddForeignKey("dbo.JobRequisition", "Employee_ID1", "dbo.Employee", "ID");
        }
    }
}
