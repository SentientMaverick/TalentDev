using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Repositories;
using TalentAcquisition.Repositories.Interfaces;

namespace TalentAcquisition.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        TalentContext _db;
        public EmployeeRepository(TalentContext db)
        {
            _db = db;
        }

        void IEmployeeRepository.DisableEmployee(Employee employee)
        {
            employee.Disabled = true;
            _db.Employees.Add(employee);
            _db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
        }
        void IEmployeeRepository.AddEmployee(Employee employee)
        {
            _db.Employees.Add(employee);
        }
        IEnumerable<Employee> IEmployeeRepository.GetAll()
        {
            return _db.Employees.Where(x => x.Disabled == false);
        }
        IEnumerable<Employee> GetAll()
        {
            return _db.Employees.Where(x => x.Disabled == false);
        }
        Employee IEmployeeRepository.GetEmployee(int Id)
        {
            return _db.Employees.Find(Id);
        }
        void IEmployeeRepository.Save()
        {
            _db.SaveChanges();
        }
        public IQueryable<Employee> GetAllQueryable()
        {
            return _db.Employees.Include("OfficePosition").Where(x=>x.Disabled==false);
        }
        IEnumerable<Employee> IEmployeeRepository.GetAllArchive()
        {
            return _db.Employees.Where(x => x.Disabled == true);
        }
    }
}