using Employee.DTOs;
using Employee.Model;

namespace Employee.Data.repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<tblEmployee>> GetEmployees();
        Task<tblEmployee> GetEmployee(int employeeId);
        Task<tblEmployee> GetEmployeeWith3rdHighestSalary();

        Task<tblEmployee> AddEmployee(EpmloyeeDtos employee);
        Task<tblEmployee> UpdateEmployee(tblEmployee employee);
        void DeleteEmployee(int employeeId);
    }
}
