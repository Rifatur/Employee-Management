using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee.Data.Migrations
{
    /// <inheritdoc />
    public partial class employee001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEmployees",
                columns: table => new
                {
                    employeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    employeeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    employeeSalary = table.Column<int>(type: "int", nullable: false),
                    supervisorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployees", x => x.employeeId);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployeeAttendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    isPresent = table.Column<int>(type: "int", nullable: false),
                    isAbsent = table.Column<int>(type: "int", nullable: false),
                    isOffday = table.Column<int>(type: "int", nullable: false),
                    tblEmployeeemployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployeeAttendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_tblEmployeeAttendances_tblEmployees_tblEmployeeemployeeId",
                        column: x => x.tblEmployeeemployeeId,
                        principalTable: "tblEmployees",
                        principalColumn: "employeeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeAttendances_tblEmployeeemployeeId",
                table: "tblEmployeeAttendances",
                column: "tblEmployeeemployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEmployeeAttendances");

            migrationBuilder.DropTable(
                name: "tblEmployees");
        }
    }
}
