namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedCashRequisitionModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CashRequisition", name: "BranchId_BranchId", newName: "Branch_BranchId");
            RenameIndex(table: "dbo.CashRequisition", name: "IX_BranchId_BranchId", newName: "IX_Branch_BranchId");
            CreateTable(
                "dbo.CashLineItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CashRequisitionNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashRequisition", t => t.CashRequisitionNo)
                .Index(t => t.CashRequisitionNo);
            
            AddColumn("dbo.CashRequisition", "TotalAmount", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashLineItem", "CashRequisitionNo", "dbo.CashRequisition");
            DropIndex("dbo.CashLineItem", new[] { "CashRequisitionNo" });
            DropColumn("dbo.CashRequisition", "TotalAmount");
            DropTable("dbo.CashLineItem");
            RenameIndex(table: "dbo.CashRequisition", name: "IX_Branch_BranchId", newName: "IX_BranchId_BranchId");
            RenameColumn(table: "dbo.CashRequisition", name: "Branch_BranchId", newName: "BranchId_BranchId");
        }
    }
}
