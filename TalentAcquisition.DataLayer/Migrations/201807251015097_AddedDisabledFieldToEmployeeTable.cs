namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDisabledFieldToEmployeeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "Disabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "Disabled");
        }
    }
}
