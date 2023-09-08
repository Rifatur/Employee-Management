using Employee.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee.Data.Context
{
    public class ApplicationDbContext : DbContext
    {

        private readonly IConfiguration configuration;
        public ApplicationDbContext(IConfiguration _config)
        {
            configuration = _config;
        }

        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "EmployeeDB");
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DataConnection"));
        }

        public DbSet<tblEmployee> tblEmployees { get; set; }
        public DbSet<tblEmployeeAttendance> tblEmployeeAttendances { get; set; }
    }
}
