using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Model
{
    public class tblEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int employeeId { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [StringLength(100)]
        public string? employeeName { get; set; }

        [Required(ErrorMessage = "Please enter Employee Code")]
        [StringLength(10)]
        public string? employeeCode { get; set; }

        [Required(ErrorMessage = "Please enter Salary")]
        public int employeeSalary { get; set; }

        public int supervisorId { get; set; }

        public ICollection<tblEmployeeAttendance>? tblEmployeeAttendances { get; set; }

    }
}
