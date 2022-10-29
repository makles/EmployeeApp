using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Data
{
    public class UserDBContext:DbContext
    {
        public DbSet<Employee> Employee { get; set; }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }
    }
}
