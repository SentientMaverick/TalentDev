namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSomeModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApprovalEntry",
                c => new
                    {
                        No = c.Int(nullable: false, identity: true),
                        ProcessNo = c.String(),
                        Sender = c.String(),
                        Approver = c.String(),
                        Sequence = c.Int(nullable: false),
                        Status = c.String(),
                        ProcessType = c.String(),
                    })
                .PrimaryKey(t => t.No);
            
            CreateTable(
                "dbo.DocumentWorkFlow",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sender = c.Int(nullable: false),
                        ProcessName = c.String(),
                        NoOfApprovals = c.Int(nullable: false),
                        Approver1Id = c.String(nullable: false),
                        Approver2Id = c.String(),
                        Approver3Id = c.String(),
                        Approver4Id = c.String(),
                        Approver5Id = c.String(),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Employee", "EmployeeNumber", c => c.String(maxLength: 50));
            CreateIndex("dbo.Employee", "EmployeeNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employee", new[] { "EmployeeNumber" });
            AlterColumn("dbo.Employee", "EmployeeNumber", c => c.String());
            DropTable("dbo.DocumentWorkFlow");
            DropTable("dbo.ApprovalEntry");
        }
    }
}
