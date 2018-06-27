namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNextOfKinDetailsToEmployeeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfessionalCertification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Year = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.WorkHistory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        CompanyName = c.String(),
                        JobTitle = c.String(),
                        JobDescription = c.String(),
                        ReasonsForLeaving = c.String(),
                        StartingDate = c.DateTime(nullable: false),
                        EndingDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.SkillEmployee",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Employee_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Employee_ID })
                .ForeignKey("dbo.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.Employee_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Employee_ID);
            
            AddColumn("dbo.Employee", "Nationality", c => c.String());
            AddColumn("dbo.Employee", "MaritalStatus", c => c.String());
            AddColumn("dbo.Employee", "NextofKinName", c => c.String());
            AddColumn("dbo.Employee", "NextofKinRelationship", c => c.String());
            AddColumn("dbo.Employee", "NextofKinAddress", c => c.String());
            AddColumn("dbo.Employee", "NextofKinContact", c => c.String());
            AddColumn("dbo.Employee", "PassportNumber", c => c.String());
            AddColumn("dbo.Employee", "TIN", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkHistory", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.ProfessionalCertification", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.SkillEmployee", "Employee_ID", "dbo.Employee");
            DropForeignKey("dbo.SkillEmployee", "Skill_ID", "dbo.Skill");
            DropIndex("dbo.SkillEmployee", new[] { "Employee_ID" });
            DropIndex("dbo.SkillEmployee", new[] { "Skill_ID" });
            DropIndex("dbo.WorkHistory", new[] { "EmployeeID" });
            DropIndex("dbo.ProfessionalCertification", new[] { "EmployeeID" });
            DropColumn("dbo.Employee", "TIN");
            DropColumn("dbo.Employee", "PassportNumber");
            DropColumn("dbo.Employee", "NextofKinContact");
            DropColumn("dbo.Employee", "NextofKinAddress");
            DropColumn("dbo.Employee", "NextofKinRelationship");
            DropColumn("dbo.Employee", "NextofKinName");
            DropColumn("dbo.Employee", "MaritalStatus");
            DropColumn("dbo.Employee", "Nationality");
            DropTable("dbo.SkillEmployee");
            DropTable("dbo.WorkHistory");
            DropTable("dbo.ProfessionalCertification");
        }
    }
}
