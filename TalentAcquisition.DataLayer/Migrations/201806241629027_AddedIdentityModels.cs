namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIdentityModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeID, t.GroupId })
                .ForeignKey("dbo.Group", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.GroupId })
                .ForeignKey("dbo.ApplicationRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ApplicationRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.Group");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.Group");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRole");
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "EmployeeID" });
            DropTable("dbo.ApplicationRole");
            DropTable("dbo.ApplicationRoleGroups");
            DropTable("dbo.Group");
            DropTable("dbo.ApplicationUserGroups");
        }
    }
}
