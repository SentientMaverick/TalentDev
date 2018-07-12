namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAppraisalKPIModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraisalKPI",
                c => new
                    {
                        AppraisalCode = c.String(nullable: false, maxLength: 128),
                        JobKPICode = c.String(nullable: false, maxLength: 128),
                        JobKPI_Id = c.Guid(),
                        Appraisal_No = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppraisalCode, t.JobKPICode })
                .ForeignKey("dbo.JobKPI", t => t.JobKPI_Id)
                .ForeignKey("dbo.Appraisal", t => t.Appraisal_No)
                .Index(t => t.JobKPI_Id)
                .Index(t => t.Appraisal_No);
            
            CreateTable(
                "dbo.Appraisal",
                c => new
                    {
                        No = c.String(nullable: false, maxLength: 128),
                        AppraisalType = c.String(),
                        AppraisalNonJobKPICode = c.String(maxLength: 128),
                        AppraisalClass = c.String(),
                        AppraisalPeriodCode = c.String(maxLength: 128),
                        AppraiseeCode = c.String(),
                        AppraisalSupervisor = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        AppraisalName = c.String(),
                        AppraisalJobTitle = c.String(),
                        AppraisalSupervisorName = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.No)
                .ForeignKey("dbo.AppraisalNonJobKPI", t => t.AppraisalNonJobKPICode)
                .ForeignKey("dbo.AppraisalPeriod", t => t.AppraisalPeriodCode)
                .Index(t => t.AppraisalNonJobKPICode)
                .Index(t => t.AppraisalPeriodCode);
            
            CreateTable(
                "dbo.AppraisalNonJobKPI",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        AppraisalClass = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.JobKPI",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AppraisalNonJobKPICode = c.String(maxLength: 128),
                        AppraisalClass = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        MaxScore = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalNonJobKPI", t => t.AppraisalNonJobKPICode)
                .Index(t => t.AppraisalNonJobKPICode);
            
            CreateTable(
                "dbo.AppraisalPeriod",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Closed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppraisalKPI", "Appraisal_No", "dbo.Appraisal");
            DropForeignKey("dbo.Appraisal", "AppraisalPeriodCode", "dbo.AppraisalPeriod");
            DropForeignKey("dbo.Appraisal", "AppraisalNonJobKPICode", "dbo.AppraisalNonJobKPI");
            DropForeignKey("dbo.AppraisalKPI", "JobKPI_Id", "dbo.JobKPI");
            DropForeignKey("dbo.JobKPI", "AppraisalNonJobKPICode", "dbo.AppraisalNonJobKPI");
            DropIndex("dbo.JobKPI", new[] { "AppraisalNonJobKPICode" });
            DropIndex("dbo.Appraisal", new[] { "AppraisalPeriodCode" });
            DropIndex("dbo.Appraisal", new[] { "AppraisalNonJobKPICode" });
            DropIndex("dbo.AppraisalKPI", new[] { "Appraisal_No" });
            DropIndex("dbo.AppraisalKPI", new[] { "JobKPI_Id" });
            DropTable("dbo.AppraisalPeriod");
            DropTable("dbo.JobKPI");
            DropTable("dbo.AppraisalNonJobKPI");
            DropTable("dbo.Appraisal");
            DropTable("dbo.AppraisalKPI");
        }
    }
}
