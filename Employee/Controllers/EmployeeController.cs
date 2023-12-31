﻿using Employee.Data.Context;
using Employee.Data.repository.Interface;
using Employee.DTOs;
using Employee.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Controllers
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(ApplicationDbContext dbContext, IEmployeeRepository employeeRepository)
        {
            _dbContext = dbContext;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("Lists")]
        public async Task<IActionResult> GetEmployee()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateEmployee(EpmloyeeDtos employee)
        {
 
            if (ModelState.IsValid)
            {
                var existingEmployeeCode = await _dbContext.tblEmployees.SingleOrDefaultAsync(e => e.employeeCode == employee.employeeCode);
                if (existingEmployeeCode != null)
                {
                    return BadRequest("Employee code already exists.");
                }

                try
                {
                    if (employee == null)
                        return BadRequest();

                    var createdEmployee = await _employeeRepository.AddEmployee(employee);

                    return CreatedAtAction(nameof(GetEmployee),
                        new { id = createdEmployee.employeeId}, createdEmployee);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error creating new employee record");
                }


            }
            return BadRequest(ModelState);


        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeDTOs employeeUpdates)
        {
            if (employeeUpdates == null)
            {
                return BadRequest();
            }

            var existingEmployee = await _dbContext.tblEmployees.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check for duplicate EmployeeCode .
            var duplicateEmployee = await _dbContext.tblEmployees.FirstOrDefaultAsync(e => e.employeeCode == employeeUpdates.employeeCode );
            if (duplicateEmployee != null)
            {
                return BadRequest("Employee code already exists.");
            }

            // Update  properties.
            existingEmployee.employeeName = employeeUpdates.employeeName;
            existingEmployee.employeeCode = employeeUpdates.employeeCode;

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        [HttpGet("third-highest-salary")]
        public async Task<IActionResult> GetEmployeeWith3rdHighestSalary()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployeeWith3rdHighestSalary());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }

            
        }
        
        [HttpGet("Employees-maximum-minimum-salary")]
        public IActionResult GetEmployeesWithNoAbsentRecords()
        {

            var query = from employee in _dbContext.tblEmployees
                        join attendance in _dbContext.tblEmployeeAttendances
                        on employee.employeeId equals attendance.employeeId into employeeAttendanceGroup
                        where employeeAttendanceGroup.All(a => a.isAbsent != 1)
                        orderby employee.employeeSalary descending // Sort by salary from max to min
                        select employee;


            return Ok(query.ToList());
        }

        [HttpGet("monthly-report")]
        public async Task<IActionResult> GenerateReport()
        {
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            var query = from emp in _dbContext.tblEmployees
                        join att in _dbContext.tblEmployeeAttendances
                        on emp.employeeId equals att.employeeId into AttendanceGroup
                        select new
                        {

                            MonthName = currentDate.ToString("MMMM"),
                            EmployeName = emp.employeeName,
                            PayableSalary = emp.employeeSalary,
                            TotalPresent = AttendanceGroup.Count(a => a.isPresent == 1),
                            TotalAbsent = AttendanceGroup.Count(a => a.isAbsent == 1),
                            TotalOffday = AttendanceGroup.Count(a => a.isOffday == 1)
                            // Include other columns as needed
                        };
            return Ok(query.ToList());
        }

        [HttpGet("GetBySupervisor/{supervisorId}")]
        public IActionResult GetemployeeBySupervisor(int supervisorId)
        {
            var employees = _dbContext.tblEmployees
                .Where(s => s.supervisorId == supervisorId)
                .Select(e => e.employeeName)
                .ToList();

            return Ok(employees);
        }

    }
}
