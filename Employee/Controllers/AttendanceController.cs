using Employee.Data.Context;
using Employee.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public AttendanceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance()
        {
            return Ok(_dbContext.tblEmployeeAttendances.ToList());
        }

        [HttpPost("CreateAttendance")]
        public async Task<IActionResult> CreateAttendance(tblEmployeeAttendance attendance)
        {

            if (ModelState.IsValid)
            {

                tblEmployeeAttendance tblEmployee = new tblEmployeeAttendance();

                tblEmployee.employeeId = attendance.employeeId;
                tblEmployee.attendanceDate = attendance.attendanceDate;
                tblEmployee.isPresent = attendance.isPresent;
                tblEmployee.isAbsent = attendance.isAbsent;
                tblEmployee.isOffday = attendance.isOffday;
                //create Employee
                await _dbContext.tblEmployeeAttendances.AddAsync(tblEmployee);
                await _dbContext.SaveChangesAsync();
                return Ok(tblEmployee);

            }
            return BadRequest(ModelState);


        }
    }
}
