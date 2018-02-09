namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateJobApplicationModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplication", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplication", "RegistrationDate");
        }
    }
}
