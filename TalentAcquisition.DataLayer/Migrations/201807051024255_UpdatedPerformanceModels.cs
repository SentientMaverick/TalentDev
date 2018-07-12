namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPerformanceModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobKPI", "AppraisalNonJobKPICode", "dbo.AppraisalNonJobKPI");
            DropIndex("dbo.JobKPI", new[] { "AppraisalNonJobKPICode" });
            DropPrimaryKey("dbo.AppraisalKPI");
            CreateTable(
                "dbo.AppraisalCategory",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.AppraisalGrade",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.NonJobKPI",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AppraisalNonJobKPICode = c.String(maxLength: 128),
                        AppraisalClass = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        MaxScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalNonJobKPI", t => t.AppraisalNonJobKPICode)
                .Index(t => t.AppraisalNonJobKPICode);
            
            CreateTable(
                "dbo.AppraisalType",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.AppraisalKPI", "JobKPIId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Appraisal", "AppraisalTypeCode", c => c.String(maxLength: 128));
            AddColumn("dbo.JobKPI", "AppraisalNo", c => c.String());
            AddColumn("dbo.JobKPI", "KPIGroupCode", c => c.String());
            AddColumn("dbo.JobKPI", "KPIGroupName", c => c.String());
            AddColumn("dbo.JobKPI", "EmployeeRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.JobKPI", "EmployeeRemark", c => c.String());
            AddColumn("dbo.JobKPI", "SupervisorRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.JobKPI", "SupervisorRemark", c => c.String());
            AddColumn("dbo.JobKPI", "Performance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.JobKPI", "MaxScore", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddPrimaryKey("dbo.AppraisalKPI", new[] { "AppraisalCode", "JobKPIId" });
            CreateIndex("dbo.Appraisal", "AppraisalTypeCode");
            AddForeignKey("dbo.Appraisal", "AppraisalTypeCode", "dbo.AppraisalType", "Code");
            DropColumn("dbo.AppraisalKPI", "JobKPICode");
            DropColumn("dbo.Appraisal", "AppraisalType");
            DropColumn("dbo.JobKPI", "AppraisalNonJobKPICode");
            DropColumn("dbo.JobKPI", "AppraisalClass");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobKPI", "AppraisalClass", c => c.String());
            AddColumn("dbo.JobKPI", "AppraisalNonJobKPICode", c => c.String(maxLength: 128));
            AddColumn("dbo.Appraisal", "AppraisalType", c => c.String());
            AddColumn("dbo.AppraisalKPI", "JobKPICode", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Appraisal", "AppraisalTypeCode", "dbo.AppraisalType");
            DropForeignKey("dbo.NonJobKPI", "AppraisalNonJobKPICode", "dbo.AppraisalNonJobKPI");
            DropIndex("dbo.NonJobKPI", new[] { "AppraisalNonJobKPICode" });
            DropIndex("dbo.Appraisal", new[] { "AppraisalTypeCode" });
            DropPrimaryKey("dbo.AppraisalKPI");
            AlterColumn("dbo.JobKPI", "MaxScore", c => c.String());
            DropColumn("dbo.JobKPI", "Performance");
            DropColumn("dbo.JobKPI", "SupervisorRemark");
            DropColumn("dbo.JobKPI", "SupervisorRating");
            DropColumn("dbo.JobKPI", "EmployeeRemark");
            DropColumn("dbo.JobKPI", "EmployeeRating");
            DropColumn("dbo.JobKPI", "KPIGroupName");
            DropColumn("dbo.JobKPI", "KPIGroupCode");
            DropColumn("dbo.JobKPI", "AppraisalNo");
            DropColumn("dbo.Appraisal", "AppraisalTypeCode");
            DropColumn("dbo.AppraisalKPI", "JobKPIId");
            DropTable("dbo.AppraisalType");
            DropTable("dbo.NonJobKPI");
            DropTable("dbo.AppraisalGrade");
            DropTable("dbo.AppraisalCategory");
            AddPrimaryKey("dbo.AppraisalKPI", new[] { "AppraisalCode", "JobKPICode" });
            CreateIndex("dbo.JobKPI", "AppraisalNonJobKPICode");
            AddForeignKey("dbo.JobKPI", "AppraisalNonJobKPICode", "dbo.AppraisalNonJobKPI", "Code");
        }
    }
}
