namespace TalentAcquisition.DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Core.Domain;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<TalentAcquisition.DataLayer.TalentContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TalentAcquisition.DataLayer.TalentContext";
        }

        protected override void Seed(TalentAcquisition.DataLayer.TalentContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //var industries = new List<Industry>
            //{
            //  new Industry {Name="Engineering"},
            //  new Industry {Name="Accounting / Audit / Tax" },
            //  new Industry {Name="Administration & Office Support" },
            //  new Industry {Name="Agriculture/Farming" },
            //  new Industry {Name="Banking / Finance / Insurance" },
            //  new Industry {Name="Building Design/Architecture" },
            //  new Industry {Name="Construction" },
            //  new Industry {Name="Consulting/Business Strategy & Planning" },
            //  new Industry {Name="Creatives(Arts, Design, Fashion)" },
            //  new Industry {Name="Customer Service" },
            //  new Industry {Name="Education/Teaching/Training" },
            //  new Industry {Name="Executive / Top Management" },
            //  new Industry {Name="Information Technology" },
            //  new Industry {Name="Oil&Gas / Mining / Energy" },
            //  new Industry {Name="Manufacturing / Production" },
            //  new Industry {Name="Healthcare / Pharmaceutical" },
            //  new Industry {Name="Project / Programme Management" },
            //  new Industry {Name="Marketing / Advertising / Communications" },
            //  new Industry {Name="Supply Chain / Procurement" },
            //  new Industry {Name="Sales/Business Development" },
            //  new Industry {Name="Telecommunications"}
            //};

            //industries.ForEach(s => context.Industries.AddOrUpdate(p => p.Name, s));

            //var departments = new List<Department>
            //{
            //    new Department {DepartmentName="Engineering"},
            //    new Department {DepartmentName="Account"},
            //    new Department {DepartmentName="Production"},
            //    new Department {DepartmentName="Human Resources"},
            //    new Department {DepartmentName="Store"},
            //    new Department {DepartmentName="Marketing"},
            //    new Department {DepartmentName="Office Administration"},
            //};

            //departments.ForEach(s => context.Departments.AddOrUpdate(p => p.DepartmentName, s));
            //var officepositions = new List<OfficePosition>
            //{
            //    new OfficePosition {DepartmentID=1,Title="Head of Engineering",
            //        RoleSummary ="The Head of Enginnering roles is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=1,Title="Head of Engineering",
            //        RoleSummary ="The Head of Enginnering role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=2,Title="Head of Account",
            //        RoleSummary ="The Head of Accounts role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=3,Title="Head of Production",
            //        RoleSummary ="The Head of Production role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=4,Title="Head of Human Resources",
            //        RoleSummary ="The Head of Human Resources role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=1,Title="Engineer(Electrical)",
            //        RoleSummary ="The Electrical Enginner role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=1,Title="Graduate Engineer",
            //        RoleSummary ="The Graduate Enginner role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=1,Title="Senior Engineer(Mechanical)",
            //        RoleSummary ="The Senior Enginner role is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            // new OfficePosition {DepartmentID=4,Title="Human Resources Officer",
            //        RoleSummary ="The Human Resources Officer is to ensure all Engineering Activities are carried out when due",IndustryID=1 },
            //};
            //officepositions.ForEach(s => context.OfficePositions.AddOrUpdate(p => p.Title, s));

            //var employees = new List<Employee>
            //{
            //    new Employee {FirstName="Mike",LastName="Abdul",EmployeeNumber="E000456",Password="password",
            //        DateOfBirth =DateTime.Parse("1975-04-15"),EmploymentDate=DateTime.Parse("2000-12-12"),OfficePositionID=1 },
            //    new Employee {FirstName="Phil",LastName="Dumphy",EmployeeNumber="E000457",Password="password",
            //        DateOfBirth =DateTime.Parse("1978-06-16"),EmploymentDate=DateTime.Parse("2000-12-12"),OfficePositionID=2 },
            //    new Employee {FirstName="James",LastName="Oliver",EmployeeNumber="E000458",Password="password",
            //        DateOfBirth =DateTime.Parse("1971-08-17"),EmploymentDate=DateTime.Parse("2000-12-12"),OfficePositionID=3 },
            //    new Employee {FirstName="Clark",LastName="Kent",EmployeeNumber="E000459",Password="password",
            //        DateOfBirth =DateTime.Parse("1965-10-18"),EmploymentDate=DateTime.Parse("2000-12-12"),OfficePositionID=4 },
            //    new Employee {FirstName="Bruce",LastName="Wayne",EmployeeNumber="E000480",Password="password",
            //        DateOfBirth =DateTime.Parse("1969-12-19"),EmploymentDate=DateTime.Parse("2000-12-12"),OfficePositionID=8 },
            //    new Employee {FirstName="Barry",LastName="Allen",EmployeeNumber="E000460",Password="password",
            //        DateOfBirth =DateTime.Parse("1977-02-20"),EmploymentDate=DateTime.Parse("2000-12-12"),OfficePositionID=7 }
            //};
            //employees.ForEach(c => context.Employees.AddOrUpdate(p => p.LastName, c));
        }
    }
}
