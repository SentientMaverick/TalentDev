namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class progressmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobRequisition", "AgeLimit", c => c.Int(nullable: false));
            AddColumn("dbo.JobRequisition", "EducationalRequirements", c => c.String());
            AddColumn("dbo.JobRequisition", "JobResponsibilities", c => c.String());
            AddColumn("dbo.JobRequisition", "YearsOfExperience", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobRequisition", "YearsOfExperience");
            DropColumn("dbo.JobRequisition", "JobResponsibilities");
            DropColumn("dbo.JobRequisition", "EducationalRequirements");
            DropColumn("dbo.JobRequisition", "AgeLimit");
        }
    }
}
