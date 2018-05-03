namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedMetricOfficeAndInterviewModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.JobRequisitionSkill", newName: "SkillJobRequisition");
            DropPrimaryKey("dbo.SkillJobRequisition");
            CreateTable(
                "dbo.ApplicantEvaluationMetrics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OfficePositionID = c.Int(nullable: false),
                        EvaluationCode = c.String(nullable: false),
                        EvaluationDescription = c.String(nullable: false),
                        MaximumScore = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OfficePosition", t => t.OfficePositionID, cascadeDelete: true)
                .Index(t => t.OfficePositionID);
            
            AlterColumn("dbo.InterviewEvaluation", "Score1", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterviewEvaluation", "Score2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterviewEvaluation", "Score3", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddPrimaryKey("dbo.SkillJobRequisition", new[] { "Skill_ID", "JobRequisition_JobRequisitionID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicantEvaluationMetrics", "OfficePositionID", "dbo.OfficePosition");
            DropIndex("dbo.ApplicantEvaluationMetrics", new[] { "OfficePositionID" });
            DropPrimaryKey("dbo.SkillJobRequisition");
            AlterColumn("dbo.InterviewEvaluation", "Score3", c => c.Int(nullable: false));
            AlterColumn("dbo.InterviewEvaluation", "Score2", c => c.Int(nullable: false));
            AlterColumn("dbo.InterviewEvaluation", "Score1", c => c.Int(nullable: false));
            DropTable("dbo.ApplicantEvaluationMetrics");
            AddPrimaryKey("dbo.SkillJobRequisition", new[] { "JobRequisition_JobRequisitionID", "Skill_ID" });
            RenameTable(name: "dbo.SkillJobRequisition", newName: "JobRequisitionSkill");
        }
    }
}
