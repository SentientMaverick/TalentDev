namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedcontextagain : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobSeeker", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobSeeker", "RegistrationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
