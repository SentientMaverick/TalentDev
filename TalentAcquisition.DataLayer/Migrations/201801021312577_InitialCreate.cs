namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobSeeker",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicantNumber = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Password = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Certification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobSeeker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeeker_ID)
                .Index(t => t.JobSeeker_ID);
            
            CreateTable(
                "dbo.JobApplication",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        JobSeekerID = c.Int(nullable: false),
                        JobRequisitionID = c.Int(nullable: false),
                        ApplicationStatus = c.Int(),
                        Employee_ID = c.Int(),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .ForeignKey("dbo.Employee", t => t.Employee_ID)
                .ForeignKey("dbo.JobRequisition", t => t.JobRequisitionID, cascadeDelete: true)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeekerID, cascadeDelete: true)
                .Index(t => t.JobSeekerID)
                .Index(t => t.JobRequisitionID)
                .Index(t => t.Employee_ID);
            
            CreateTable(
                "dbo.JobRequisition",
                c => new
                    {
                        JobRequisitionID = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        JobDescription = c.String(),
                        HumanResourcePersonnelID = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobRequisitionID)
                .ForeignKey("dbo.Employee", t => t.HumanResourcePersonnelID, cascadeDelete: true)
                .Index(t => t.HumanResourcePersonnelID);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeNumber = c.String(),
                        EmploymentDate = c.DateTime(nullable: false),
                        OfficePositionID = c.Int(nullable: false),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Password = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Department_DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Department", t => t.Department_DepartmentID)
                .ForeignKey("dbo.OfficePosition", t => t.OfficePositionID, cascadeDelete: true)
                .Index(t => t.OfficePositionID)
                .Index(t => t.Department_DepartmentID);
            
            CreateTable(
                "dbo.Interview",
                c => new
                    {
                        InterviewID = c.Int(nullable: false, identity: true),
                        JobRequisitionID = c.Int(nullable: false),
                        JobApplicationID = c.Int(nullable: false),
                        OfficePositionID = c.Int(nullable: false),
                        ProposedDate1 = c.DateTime(nullable: false),
                        ProposedDate2 = c.DateTime(nullable: false),
                        ScheduledDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InterviewID);
            
            CreateTable(
                "dbo.InterviewDetail",
                c => new
                    {
                        InterviewDetailID = c.Int(nullable: false),
                        TeamMember1ID = c.Int(nullable: false),
                        TeamMember2ID = c.Int(nullable: false),
                        TeamMember3ID = c.Int(nullable: false),
                        TeamMember4ID = c.Int(nullable: false),
                        TeamMember1Recommendation = c.Boolean(),
                        TeamMember2Recommendation = c.Boolean(),
                        TeamMember3Recommendation = c.Boolean(),
                        TeamMember4Recommendation = c.Boolean(),
                        ApplicantStrengthE1 = c.String(),
                        ApplicantStrengthE2 = c.String(),
                        ApplicantStrengthE3 = c.String(),
                        ApplicantStrengthE4 = c.String(),
                        ApplicantWeaknessE1 = c.String(),
                        ApplicantWeaknessE2 = c.String(),
                        ApplicantWeaknessE3 = c.String(),
                        ApplicantWeaknessE4 = c.String(),
                    })
                .PrimaryKey(t => t.InterviewDetailID)
                .ForeignKey("dbo.Interview", t => t.InterviewDetailID)
                .Index(t => t.InterviewDetailID);
            
            CreateTable(
                "dbo.OfficePosition",
                c => new
                    {
                        OfficePositionID = c.Int(nullable: false, identity: true),
                        DepartmentID = c.Int(nullable: false),
                        Title = c.String(),
                        RoleSummary = c.String(),
                        Reqirements = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OfficePositionID)
                .ForeignKey("dbo.Department", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobSeeker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeeker_ID)
                .Index(t => t.JobSeeker_ID);
            
            CreateTable(
                "dbo.WorkExperience",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobSeeker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobSeeker", t => t.JobSeeker_ID)
                .Index(t => t.JobSeeker_ID);
            
            CreateTable(
                "dbo.InterviewEmployee",
                c => new
                    {
                        Interview_InterviewID = c.Int(nullable: false),
                        Employee_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Interview_InterviewID, t.Employee_ID })
                .ForeignKey("dbo.Interview", t => t.Interview_InterviewID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.Employee_ID, cascadeDelete: true)
                .Index(t => t.Interview_InterviewID)
                .Index(t => t.Employee_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkExperience", "JobSeeker_ID", "dbo.JobSeeker");
            DropForeignKey("dbo.School", "JobSeeker_ID", "dbo.JobSeeker");
            DropForeignKey("dbo.JobApplication", "JobSeekerID", "dbo.JobSeeker");
            DropForeignKey("dbo.JobApplication", "JobRequisitionID", "dbo.JobRequisition");
            DropForeignKey("dbo.JobRequisition", "HumanResourcePersonnelID", "dbo.Employee");
            DropForeignKey("dbo.Employee", "OfficePositionID", "dbo.OfficePosition");
            DropForeignKey("dbo.OfficePosition", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Employee", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.JobApplication", "Employee_ID", "dbo.Employee");
            DropForeignKey("dbo.InterviewEmployee", "Employee_ID", "dbo.Employee");
            DropForeignKey("dbo.InterviewEmployee", "Interview_InterviewID", "dbo.Interview");
            DropForeignKey("dbo.InterviewDetail", "InterviewDetailID", "dbo.Interview");
            DropForeignKey("dbo.Certification", "JobSeeker_ID", "dbo.JobSeeker");
            DropIndex("dbo.InterviewEmployee", new[] { "Employee_ID" });
            DropIndex("dbo.InterviewEmployee", new[] { "Interview_InterviewID" });
            DropIndex("dbo.WorkExperience", new[] { "JobSeeker_ID" });
            DropIndex("dbo.School", new[] { "JobSeeker_ID" });
            DropIndex("dbo.OfficePosition", new[] { "DepartmentID" });
            DropIndex("dbo.InterviewDetail", new[] { "InterviewDetailID" });
            DropIndex("dbo.Employee", new[] { "Department_DepartmentID" });
            DropIndex("dbo.Employee", new[] { "OfficePositionID" });
            DropIndex("dbo.JobRequisition", new[] { "HumanResourcePersonnelID" });
            DropIndex("dbo.JobApplication", new[] { "Employee_ID" });
            DropIndex("dbo.JobApplication", new[] { "JobRequisitionID" });
            DropIndex("dbo.JobApplication", new[] { "JobSeekerID" });
            DropIndex("dbo.Certification", new[] { "JobSeeker_ID" });
            DropTable("dbo.InterviewEmployee");
            DropTable("dbo.WorkExperience");
            DropTable("dbo.School");
            DropTable("dbo.Department");
            DropTable("dbo.OfficePosition");
            DropTable("dbo.InterviewDetail");
            DropTable("dbo.Interview");
            DropTable("dbo.Employee");
            DropTable("dbo.JobRequisition");
            DropTable("dbo.JobApplication");
            DropTable("dbo.Certification");
            DropTable("dbo.JobSeeker");
        }
    }
}
