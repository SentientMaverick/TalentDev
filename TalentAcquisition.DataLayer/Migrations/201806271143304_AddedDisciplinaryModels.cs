namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDisciplinaryModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisciplinaryAction",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.DisciplinaryCase",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CaseNumber = c.String(),
                        EmployeeNumber = c.String(),
                        EmployeeName = c.String(),
                        JobTitleCode = c.String(),
                        JobTitleName = c.String(),
                        ComplaintDate = c.DateTime(nullable: false),
                        IndisciplineTypeCode = c.String(maxLength: 128),
                        DisciplinaryActionCode = c.String(maxLength: 128),
                        ActionDetails = c.String(),
                        ActionStartDate = c.DateTime(nullable: false),
                        ActionEndDate = c.DateTime(nullable: false),
                        Reasons = c.String(),
                        Posted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisciplinaryAction", t => t.DisciplinaryActionCode)
                .ForeignKey("dbo.IndisciplineType", t => t.IndisciplineTypeCode)
                .Index(t => t.IndisciplineTypeCode)
                .Index(t => t.DisciplinaryActionCode);
            
            CreateTable(
                "dbo.IndisciplineType",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DisciplinaryCase", "IndisciplineTypeCode", "dbo.IndisciplineType");
            DropForeignKey("dbo.DisciplinaryCase", "DisciplinaryActionCode", "dbo.DisciplinaryAction");
            DropIndex("dbo.DisciplinaryCase", new[] { "DisciplinaryActionCode" });
            DropIndex("dbo.DisciplinaryCase", new[] { "IndisciplineTypeCode" });
            DropTable("dbo.IndisciplineType");
            DropTable("dbo.DisciplinaryCase");
            DropTable("dbo.DisciplinaryAction");
        }
    }
}
