using EmployeeApp.Data;
using EmployeeApp.IRepository;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Repository
{
    public class EmployeeRepostitory : IEmployeeRepository
    {

        private readonly UserDBContext _db;

        public EmployeeRepostitory(UserDBContext db)
        {
            _db = db;
        }
        public bool CreateEmployee(Employee employee)
        {
            _db.Employee.Add(employee);
            return Save();
        }

        public bool DeleteEmployee(Employee employee)
        {
            _db.Employee.Remove(employee);
            return Save();
        }

        public IQueryable<Employee> GetEmployee()
        {
            return _db.Employee.AsQueryable();

        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateEmployee(Employee employee)
        {
            _db.Employee.Update(employee);
            return Save();
        }


        public bool EmployeeExists(int id)
        {
            return _db.Employee.Any(x => x.Id == id);
        }

        public Employee GetEmployee(int id)
        {
            return _db.Employee.FirstOrDefault(x => x.Id == id);
        }

        public bool EmployeeExists(string title)
        {
            bool value = _db.Employee.Any(y => y.Name.ToLower().Trim() == title.ToLower().Trim());
            return value;
        }


        public bool DeleteMulEmployee(List<Employee> employees)
        {
            
                foreach (var item in employees)
                {
                    Employee obj = new Employee();
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Email = item.Email;
                    obj.Phone = item.Phone;
                    obj.Address = item.Address;
                    var entry = _db.Entry(obj);
                    if (entry.State == EntityState.Detached) _db.Employee.Attach(obj);
                    _db.Employee.Remove(obj);
                }
                //int i = _db.SaveChanges();
                //if (i > 0)
                //{
                //    str = "Records has been deleted";
                //}
                //else
                //{
                //    str = "Records deletion has been faild";
                //}
           
            return Save();
        }


    }
}
