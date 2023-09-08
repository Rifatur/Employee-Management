using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee.Data.Migrations
{
    /// <inheritdoc />
    public partial class employee002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "attendanceDate",
                table: "tblEmployeeAttendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attendanceDate",
                table: "tblEmployeeAttendances");
        }
    }
}
