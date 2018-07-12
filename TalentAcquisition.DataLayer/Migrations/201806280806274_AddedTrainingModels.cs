namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTrainingModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainingCourse",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.TrainingProvider",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.TrainingQuestion",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                        Remarks = c.String(),
                        Training_ApplicationNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Training", t => t.Training_ApplicationNo)
                .Index(t => t.Training_ApplicationNo);
            
            CreateTable(
                "dbo.Training",
                c => new
                    {
                        ApplicationNo = c.String(nullable: false, maxLength: 128),
                        ApplicationDate = c.DateTime(nullable: false),
                        EmployeeNo = c.String(),
                        TrainingGroupNo = c.Int(nullable: false),
                        ParticipantNo = c.Int(nullable: false),
                        TrainingCourseCode = c.String(maxLength: 128),
                        EmployeeName = c.String(),
                        EmployeeDepartment = c.String(),
                        Description = c.String(),
                        Purpose = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        StopDate = c.DateTime(nullable: false),
                        Duration = c.Single(nullable: false),
                        DurationUnits = c.String(),
                        Location = c.String(),
                        TrainingProviderCode = c.String(maxLength: 128),
                        ProviderName = c.String(),
                        EstimatedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationNo)
                .ForeignKey("dbo.TrainingCourse", t => t.TrainingCourseCode)
                .ForeignKey("dbo.TrainingProvider", t => t.TrainingProviderCode)
                .Index(t => t.TrainingCourseCode)
                .Index(t => t.TrainingProviderCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainingQuestion", "Training_ApplicationNo", "dbo.Training");
            DropForeignKey("dbo.Training", "TrainingProviderCode", "dbo.TrainingProvider");
            DropForeignKey("dbo.Training", "TrainingCourseCode", "dbo.TrainingCourse");
            DropIndex("dbo.Training", new[] { "TrainingProviderCode" });
            DropIndex("dbo.Training", new[] { "TrainingCourseCode" });
            DropIndex("dbo.TrainingQuestion", new[] { "Training_ApplicationNo" });
            DropTable("dbo.Training");
            DropTable("dbo.TrainingQuestion");
            DropTable("dbo.TrainingProvider");
            DropTable("dbo.TrainingCourse");
        }
    }
}
