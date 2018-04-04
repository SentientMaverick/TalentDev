namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedNada : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Evaluation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InterviewEvaluationID = c.Int(nullable: false),
                        EvaluationCode = c.String(),
                        EvaluationDescription = c.String(),
                        Score1 = c.Int(nullable: false),
                        Score2 = c.Int(nullable: false),
                        Score3 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.InterviewEvaluation", t => t.InterviewEvaluationID, cascadeDelete: true)
                .Index(t => t.InterviewEvaluationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Evaluation", "InterviewEvaluationID", "dbo.InterviewEvaluation");
            DropIndex("dbo.Evaluation", new[] { "InterviewEvaluationID" });
            DropTable("dbo.Evaluation");
        }
    }
}
