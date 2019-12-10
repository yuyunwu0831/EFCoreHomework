using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeWork1.models;

namespace HomeWork1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.ToListAsync();
        }

        // GET: api/Courses/ViewStudent:20191209:yuyun:新增用來查詢檢視表VwCourseStudents的內容
        [HttpGet("ViewStudent")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetvwCourseStudents()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }

        // GET: api/Courses/ViewStudentCount:20191209:yuyun:新增用來查詢檢視表VwCourseStudentCount的內容
        [HttpGet("ViewStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetvwCourseStudentCount()
        {
            return await _context.VwCourseStudentCount.ToListAsync();
        }

        // GET: api/Courses/ViewDCCount :20191209:yuyun:新增用 Raw SQL Query 的方式查詢檢視表vwDepartmentCourseCount 的內容
        [HttpGet("ViewDCCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetvwDepartmentCourseCount()
        {
            //查詢方法一
            //var vwDCCount = await _context.VwDepartmentCourseCount
            //    .FromSqlRaw("select * from dbo.VwDepartmentCourseCount")
            //   .ToListAsync();
            //return vwDCCount;

            //查詢方法二
            var vwDCCount = await _context.VwDepartmentCourseCount
                .FromSqlInterpolated($"select * from dbo.VwDepartmentCourseCount")
                .ToListAsync();
            return vwDCCount;
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

               

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
