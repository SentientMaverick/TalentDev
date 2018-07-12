namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCashModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashRequisition",
                c => new
                    {
                        No = c.String(nullable: false, maxLength: 128),
                        CashRequisitionTypeCode = c.String(maxLength: 128),
                        RequestCreator = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                        BranchCode = c.String(),
                        DepartmentId = c.String(),
                        Status = c.String(),
                        BranchId_BranchId = c.String(maxLength: 128),
                        Department_DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.No)
                .ForeignKey("dbo.Branch", t => t.BranchId_BranchId)
                .ForeignKey("dbo.CashRequisitionType", t => t.CashRequisitionTypeCode)
                .ForeignKey("dbo.Department", t => t.Department_DepartmentID)
                .Index(t => t.CashRequisitionTypeCode)
                .Index(t => t.BranchId_BranchId)
                .Index(t => t.Department_DepartmentID);
            
            CreateTable(
                "dbo.CashRequisitionType",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashRequisition", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.CashRequisition", "CashRequisitionTypeCode", "dbo.CashRequisitionType");
            DropForeignKey("dbo.CashRequisition", "BranchId_BranchId", "dbo.Branch");
            DropIndex("dbo.CashRequisition", new[] { "Department_DepartmentID" });
            DropIndex("dbo.CashRequisition", new[] { "BranchId_BranchId" });
            DropIndex("dbo.CashRequisition", new[] { "CashRequisitionTypeCode" });
            DropTable("dbo.CashRequisitionType");
            DropTable("dbo.CashRequisition");
        }
    }
}
