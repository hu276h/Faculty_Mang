using Faculty_M.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Faculty_M.Controllers
{

    public class HomeController : Controller
    {
        private readonly FacultyDbContext facultyDbContext;

        public HomeController(FacultyDbContext context)
        {
            facultyDbContext = context;
        }

        public IActionResult Index()
        {
            var studentCount = facultyDbContext.Students.Count();
            var instructorCount = facultyDbContext.Instructors.Count();
            var courseCount = facultyDbContext.Courses.Count();
            var departmentCount = facultyDbContext.Departments.Count();
            var scheduleCount = facultyDbContext.Schedules.Count();

            ViewBag.StudentCount = studentCount;
            ViewBag.InstructorCount = instructorCount;
            ViewBag.CourseCount = courseCount;
            ViewBag.DepartmentCount = departmentCount;
            ViewBag.ScheduleCount = scheduleCount;

            return View();
        }
    }
}