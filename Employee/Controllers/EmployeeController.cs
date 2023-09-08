using Employee.Data.Context;
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

        [HttpGet("{supervisorId}")]
        public IActionResult GetemployeeBySupervisor(int supervisorId)
        {
            var employees = _dbContext.tblEmployees
                .Where(s => s.supervisorId == supervisorId)
                .ToList();

            return Ok(employees);
        }


        [HttpGet("monthly-report")]
        public async Task<IActionResult> GenerateReport()
        {
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;

            var report = _dbContext.tblEmployeeAttendances
                .Where(a => a.attendanceDate.Month == currentMonth)
                .GroupBy(a => a.employeeId)
                .Select(g => new
                {
                    EmployeeName = _dbContext.tblEmployees.FirstOrDefault(e => e.employeeId == g.Key).employeeName,
                    MonthName = currentDate.ToString("MMMM"),
                    PayableSalary = _dbContext.tblEmployees.FirstOrDefault(e => e.employeeId == g.Key).employeeSalary,
                    //TotalPresent = g.Count(a => a.isPresent),
                    //TotalAbsent = g.Count(a => a.isAbsent),
                    TotalOffday = DateTime.DaysInMonth(currentDate.Year, currentMonth) - g.Count()
                })
                .ToList();

            return Ok(report);
        }

        [HttpGet("GetEmployeesWithNoAbsent")]
        public IActionResult GetEmployeesWithNoAbsentRecords()
        {
            var employeesWithNoAbsentRecords = _dbContext.tblEmployees
                // Filter employees with no absent rnecords
                .OrderByDescending(e => e.employeeSalary ).ToList();

            return Ok(employeesWithNoAbsentRecords);
        }



    }
}
