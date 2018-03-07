namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVirtual1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Skill", "JobRequisition_JobRequisitionID", "dbo.JobRequisition");
            DropForeignKey("dbo.Skill", "JobSeeker_ID", "dbo.JobSeeker");
            DropIndex("dbo.Skill", new[] { "JobRequisition_JobRequisitionID" });
            DropIndex("dbo.Skill", new[] { "JobSeeker_ID" });
            CreateTable(
                "dbo.JobRequisitionSkill",
                c => new
                    {
                        JobRequisition_JobRequisitionID = c.Int(nullable: false),
                        Skill_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobRequisition_JobRequisitionID, t.Skill_ID })
                .ForeignKey("dbo.JobRequisition", t => t.JobRequisition_JobRequisitionID, cascadeDelete: true)
                .ForeignKey("dbo.Skill", t => t.Skill_ID, cascadeDelete: true)
                .Index(t => t.JobRequisition_JobRequisitionID)
                .Index(t => t.Skill_ID);
            
            CreateTable(
                "dbo.SkillJobSeeker",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        JobSeeker_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.JobSeeker_ID })
                .ForeignKey("dbo.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeeker_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.JobSeeker_ID);
            
            DropColumn("dbo.Skill", "JobRequisition_JobRequisitionID");
            DropColumn("dbo.Skill", "JobSeeker_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skill", "JobSeeker_ID", c => c.Int());
            AddColumn("dbo.Skill", "JobRequisition_JobRequisitionID", c => c.Int());
            DropForeignKey("dbo.SkillJobSeeker", "JobSeeker_ID", "dbo.JobSeeker");
            DropForeignKey("dbo.SkillJobSeeker", "Skill_ID", "dbo.Skill");
            DropForeignKey("dbo.JobRequisitionSkill", "Skill_ID", "dbo.Skill");
            DropForeignKey("dbo.JobRequisitionSkill", "JobRequisition_JobRequisitionID", "dbo.JobRequisition");
            DropIndex("dbo.SkillJobSeeker", new[] { "JobSeeker_ID" });
            DropIndex("dbo.SkillJobSeeker", new[] { "Skill_ID" });
            DropIndex("dbo.JobRequisitionSkill", new[] { "Skill_ID" });
            DropIndex("dbo.JobRequisitionSkill", new[] { "JobRequisition_JobRequisitionID" });
            DropTable("dbo.SkillJobSeeker");
            DropTable("dbo.JobRequisitionSkill");
            CreateIndex("dbo.Skill", "JobSeeker_ID");
            CreateIndex("dbo.Skill", "JobRequisition_JobRequisitionID");
            AddForeignKey("dbo.Skill", "JobSeeker_ID", "dbo.JobSeeker", "ID");
            AddForeignKey("dbo.Skill", "JobRequisition_JobRequisitionID", "dbo.JobRequisition", "JobRequisitionID");
        }
    }
}
