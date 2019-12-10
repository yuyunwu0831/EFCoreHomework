using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeWork1.models;
using Microsoft.Data.SqlClient;

namespace HomeWork1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public DepartmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Department.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            //原本的PUT的內容
            //_context.Entry(department).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!DepartmentExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            //return NoContent();
            //原本的PUT的內容


            //使用Stored Procedure 進行修改資料

            // var id1 = new SqlParameter("@DepartmentID", department.DepartmentId);
            var id1 = new SqlParameter("@DepartmentID", id);
            var name = new SqlParameter("@name", department.Name);
            var Budget = new SqlParameter("@Budget", department.Budget);
            var StartDate = new SqlParameter("@StartDate", department.StartDate);
            var InstructorID = new SqlParameter("@InstructorID", department.InstructorId);
            var RowVersion_Original = new SqlParameter("@RowVersion_Original", department.RowVersion);
           

            var DeptData = await _context.Department
                .FromSqlRaw("EXECUTE dbo.Department_Update @DepartmentID,@name,@Budget,@StartDate,@InstructorID,@RowVersion_Original", parameters: new[] { id1, name, Budget, StartDate, InstructorID, RowVersion_Original })
                .ToListAsync();

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost("{name1},{Budget1},{StartDate1},{InstructorID}")]
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {

            //原本的POST內容
            //    _context.Department.Add(department);
            //    await _context.SaveChangesAsync();
            //    return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);

            //使用Stored Procedure 進行新增資料
            var name = new SqlParameter("@name",department.Name);
            var Budget = new SqlParameter("@Budget", department.Budget);
            var StartDate = new SqlParameter("@StartDate", department.StartDate);
            var InstructorID = new SqlParameter("@InstructorID",department.InstructorId);
           
            var DeptData = await _context.Department
                .FromSqlRaw("EXECUTE dbo.Department_Insert @name,@Budget,@StartDate,@InstructorID",parameters: new[] { name, Budget, StartDate, InstructorID })
                .ToListAsync();
            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);

        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            //====原本的Delete語法====
            //_context.Department.Remove(department);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);

            //====原本的Delete語法====


            //使用Stored Procedure 進行刪除資料
            var DepartmentID = new SqlParameter("@DepartmentID", department.DepartmentId);
            var RowVersion_Original = new SqlParameter("@RowVersion_Original", department.RowVersion);

            var DeptData = await _context.Department
               .FromSqlRaw("EXECUTE dbo.Department_delete @DepartmentID,@RowVersion_Original", parameters: new[] { DepartmentID, RowVersion_Original })
               .ToListAsync();
           

            return department;
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }
    }
}
