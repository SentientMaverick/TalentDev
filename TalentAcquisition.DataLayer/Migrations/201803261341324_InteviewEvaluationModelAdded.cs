namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InteviewEvaluationModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchedApplicant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobRequisitionID = c.Int(nullable: false),
                        JobSeekerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobRequisition", t => t.JobRequisitionID, cascadeDelete: true)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeekerID, cascadeDelete: true)
                .Index(t => t.JobRequisitionID)
                .Index(t => t.JobSeekerID);
            
            CreateTable(
                "dbo.InterviewEvaluation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EvaluationNo = c.String(),
                        EmployeeID = c.Int(nullable: false),
                        InterviewID = c.Int(nullable: false),
                        StageID = c.Int(nullable: false),
                        Score1 = c.Int(nullable: false),
                        Score2 = c.Int(nullable: false),
                        Score3 = c.Int(nullable: false),
                        ApplicantStrength = c.String(),
                        ApplicantWeakness = c.String(),
                        Recommendation = c.String(),
                        RecommendForHire = c.Boolean(nullable: false),
                        RecommendForStage2 = c.Boolean(nullable: false),
                        RecommendForStage3 = c.Boolean(nullable: false),
                        JobAcceptance = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Interview", t => t.InterviewID, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.InterviewID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterviewEvaluation", "InterviewID", "dbo.Interview");
            DropForeignKey("dbo.InterviewEvaluation", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.MatchedApplicant", "JobSeekerID", "dbo.JobSeeker");
            DropForeignKey("dbo.MatchedApplicant", "JobRequisitionID", "dbo.JobRequisition");
            DropIndex("dbo.InterviewEvaluation", new[] { "InterviewID" });
            DropIndex("dbo.InterviewEvaluation", new[] { "EmployeeID" });
            DropIndex("dbo.MatchedApplicant", new[] { "JobSeekerID" });
            DropIndex("dbo.MatchedApplicant", new[] { "JobRequisitionID" });
            DropTable("dbo.InterviewEvaluation");
            DropTable("dbo.MatchedApplicant");
        }
    }
}
