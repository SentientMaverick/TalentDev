namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVirtual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skill", "JobRequisition_JobRequisitionID", c => c.Int());
            CreateIndex("dbo.Skill", "JobRequisition_JobRequisitionID");
            AddForeignKey("dbo.Skill", "JobRequisition_JobRequisitionID", "dbo.JobRequisition", "JobRequisitionID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skill", "JobRequisition_JobRequisitionID", "dbo.JobRequisition");
            DropIndex("dbo.Skill", new[] { "JobRequisition_JobRequisitionID" });
            DropColumn("dbo.Skill", "JobRequisition_JobRequisitionID");
        }
    }
}
