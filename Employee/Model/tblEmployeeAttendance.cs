using System.ComponentModel.DataAnnotations;

namespace Employee.Model
{
    public class tblEmployeeAttendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public int employeeId { get; set; }
        public DateTime attendanceDate { get; set; }
        public int isPresent { get; set; }
        public int isAbsent { get; set; }
        public int isOffday { get; set; }
     
    }
}
