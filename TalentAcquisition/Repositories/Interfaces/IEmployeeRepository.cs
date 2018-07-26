using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> GetAllArchive();
        IQueryable<Employee> GetAllQueryable();
        Employee GetEmployee(int Id);
        void AddEmployee(Employee employee);
        void DisableEmployee(Employee employee);
        void Save();
    }
}