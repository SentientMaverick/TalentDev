namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedSkillModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IndustryId = c.Int(),
                        JobSeeker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Industry", t => t.IndustryId)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeeker_ID)
                .Index(t => t.IndustryId)
                .Index(t => t.JobSeeker_ID);
            
            AddColumn("dbo.JobSeeker", "HighestQualification", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skill", "JobSeeker_ID", "dbo.JobSeeker");
            DropForeignKey("dbo.Skill", "IndustryId", "dbo.Industry");
            DropIndex("dbo.Skill", new[] { "JobSeeker_ID" });
            DropIndex("dbo.Skill", new[] { "IndustryId" });
            DropColumn("dbo.JobSeeker", "HighestQualification");
            DropTable("dbo.Skill");
        }
    }
}
