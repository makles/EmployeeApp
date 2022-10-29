using EmployeeApp.Models;

namespace EmployeeApp.IRepository
{
    public interface IEmployeeRepository
    {

        IQueryable<Employee> GetEmployee();

        Employee GetEmployee(int id);

        bool EmployeeExists(int id);

        bool EmployeeExists(string title);

        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);

        bool DeleteEmployee(Employee employee);

        bool DeleteMulEmployee(List<Employee> employee);

        bool Save();
    }
}
