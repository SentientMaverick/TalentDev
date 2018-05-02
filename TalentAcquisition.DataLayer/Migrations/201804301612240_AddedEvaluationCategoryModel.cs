namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEvaluationCategoryModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EvaluationCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InterviewID = c.Int(nullable: false),
                        EvaluationCode = c.String(nullable: false),
                        EvaluationDescription = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Interview", t => t.InterviewID, cascadeDelete: true)
                .Index(t => t.InterviewID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EvaluationCategory", "InterviewID", "dbo.Interview");
            DropIndex("dbo.EvaluationCategory", new[] { "InterviewID" });
            DropTable("dbo.EvaluationCategory");
        }
    }
}
