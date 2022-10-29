using EmployeeApp.Data;
using EmployeeApp.IRepository;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserDBContext _db;
        private readonly IEmployeeRepository _employee;
        public EmployeeController(IEmployeeRepository employee, UserDBContext db)
        {
            _employee = employee;
            _db = db;
        }

        /// <summary>
        /// </summary>
        [HttpGet]

        public IQueryable Get()
        {
            return _employee.GetEmployee();
        }
        [HttpGet("{EmployeeId:int}")]
        public Employee Get(int EmployeeId)
        {
            return _employee.GetEmployee(EmployeeId);
        }

        /// <summary>
        /// Create a new movie
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest(ModelState);
            if (_employee.EmployeeExists(employee.Name))
            {
                ModelState.AddModelError("", "Employee already Exist");
                return StatusCode(500, ModelState);
            }

            if (!_employee.CreateEmployee(employee))
            {
                ModelState.AddModelError("", $"Something went wrong while saving Employee record of {employee.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok(employee);

        }

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <return></return>
        [HttpPut("{EmployeeId:int}")]
        public IActionResult Update(int EmployeeId, [FromBody] Employee employee)
        {
            if (employee == null || EmployeeId != employee.Id)
                return BadRequest(ModelState);

            if (!_employee.UpdateEmployee(employee))
            {
                ModelState.AddModelError("", $"Something went wrong while updating Employee : {employee.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <return></return>

        [HttpDelete("{EmployeeId:int}")]
        public IActionResult Delete(int EmployeeId)
        {
            if (!_employee.EmployeeExists(EmployeeId))
            {
                return NotFound();
            }

            var Employeeobj = _employee.GetEmployee(EmployeeId);

            if (!_employee.DeleteEmployee(Employeeobj))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting Employee : {Employeeobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //[HttpPost]
        //public IActionResult DeleteMultiple(List<Employee> employees)
        //{
        //    bool result = true;
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        result = _employee.DeleteMulEmployee(employees);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("DeleteRecord")]
        public IActionResult DeleteMulRecord(List<Employee> employees)
        {
            string result = "";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                result = DeleteData(employees);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok(result);
        }
        private string DeleteData(List<Employee> employees)
        {
            string str = "";
            try
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
                int i = _db.SaveChanges();
                if (i > 0)
                {
                    str = "Records has been deleted";
                }
                else
                {
                    str = "Records deletion has been faild";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return str;
        }

    }
}
