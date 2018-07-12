namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedIndexfromEmployee : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employee", new[] { "EmployeeNumber" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Employee", "EmployeeNumber");
        }
    }
}
