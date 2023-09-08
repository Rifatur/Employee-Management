using Employee.Data.Context;
using Employee.Data.repository.Interface;
using Employee.DTOs;
using Employee.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee.Data.repository.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<tblEmployee> AddEmployee(EpmloyeeDtos employee)
        {
            tblEmployee tblEmployee = new tblEmployee();

            tblEmployee.employeeName = employee.employeeName;
            tblEmployee.employeeCode = employee.employeeCode;
            tblEmployee.employeeSalary = employee.employeeSalary;
            tblEmployee.supervisorId = employee.supervisorId;
            //create Employee
            var result = await _dbContext.tblEmployees.AddAsync(tblEmployee);
                await _dbContext.SaveChangesAsync();
                return result.Entity;

        }
        

        public void DeleteEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<tblEmployee> GetEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<tblEmployee>> GetEmployees()
        {
            return await _dbContext.tblEmployees.ToListAsync();
        }

        public async Task<tblEmployee> GetEmployeeWith3rdHighestSalary()
        {
            var thirdHighestSalaryEmployee = await _dbContext.tblEmployees
                                   .OrderByDescending(e => e.employeeSalary)
                                   .Skip(2) // Skip the first two highest salaries to get the 3rd highest
                                   .FirstOrDefaultAsync();

            return thirdHighestSalaryEmployee;
        }

        public Task<tblEmployee> UpdateEmployee(tblEmployee employee)
        {
            throw new NotImplementedException();
        }
    }
}
