namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGrievanceModelsToContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GrievanceAction",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.GrievanceReport",
                c => new
                    {
                        No = c.String(nullable: false, maxLength: 128),
                        EmployeeNumber = c.String(),
                        EmployeeName = c.String(),
                        GrievanceCode = c.String(),
                        GrievanceDescription = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateApproved = c.DateTime(nullable: false),
                        OffenderCode = c.String(),
                        OffenderName = c.String(),
                        Document = c.String(),
                    })
                .PrimaryKey(t => t.No);
            
            CreateTable(
                "dbo.GrievanceType",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GrievanceType");
            DropTable("dbo.GrievanceReport");
            DropTable("dbo.GrievanceAction");
        }
    }
}
