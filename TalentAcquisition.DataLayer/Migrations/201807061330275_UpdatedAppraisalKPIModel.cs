namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedAppraisalKPIModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppraisalKPI", "Appraisal_No", "dbo.Appraisal");
            DropIndex("dbo.AppraisalKPI", new[] { "Appraisal_No" });
            RenameColumn(table: "dbo.AppraisalKPI", name: "Appraisal_No", newName: "AppraisalNo");
            DropPrimaryKey("dbo.AppraisalKPI");
            AlterColumn("dbo.AppraisalKPI", "AppraisalNo", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AppraisalKPI", new[] { "AppraisalNo", "JobKPIId" });
            CreateIndex("dbo.AppraisalKPI", "AppraisalNo");
            AddForeignKey("dbo.AppraisalKPI", "AppraisalNo", "dbo.Appraisal", "No", cascadeDelete: true);
            DropColumn("dbo.AppraisalKPI", "AppraisalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppraisalKPI", "AppraisalCode", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.AppraisalKPI", "AppraisalNo", "dbo.Appraisal");
            DropIndex("dbo.AppraisalKPI", new[] { "AppraisalNo" });
            DropPrimaryKey("dbo.AppraisalKPI");
            AlterColumn("dbo.AppraisalKPI", "AppraisalNo", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.AppraisalKPI", new[] { "AppraisalCode", "JobKPIId" });
            RenameColumn(table: "dbo.AppraisalKPI", name: "AppraisalNo", newName: "Appraisal_No");
            CreateIndex("dbo.AppraisalKPI", "Appraisal_No");
            AddForeignKey("dbo.AppraisalKPI", "Appraisal_No", "dbo.Appraisal", "No");
        }
    }
}
