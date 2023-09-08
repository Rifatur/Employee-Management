namespace Employee.DTOs
{
    public class MonthlyAttendanceReportDTOs
    {
        public string EmployeeName { get; set; }
        public string MonthName { get; set; }
        public decimal PayableSalary { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalOffday { get; set; }
    }
}
