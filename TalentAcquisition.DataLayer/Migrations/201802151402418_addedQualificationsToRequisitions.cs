namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedQualificationsToRequisitions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobRequisition", "HighestQualification", c => c.Int());
            AddColumn("dbo.JobRequisition", "MinimumQualification", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobRequisition", "MinimumQualification");
            DropColumn("dbo.JobRequisition", "HighestQualification");
        }
    }
}
