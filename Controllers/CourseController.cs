using Faculty_M.Models;
using Faculty_M.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Faculty_M.Controllers
{
    public class CourseController : Controller
    {
        private readonly FacultyDbContext facultyDbContext;
        public CourseController(FacultyDbContext Context)
        {
            facultyDbContext = Context;
        }
        public IActionResult Index()
        {
            ICollection<Course> courses = facultyDbContext.Courses.ToList();
            return View(courses);
        }

        public IActionResult Create()
        {
            var departments = facultyDbContext.Departments.ToList();
            ViewBag.DeptId = new SelectList(departments, "DeptId", "DeptName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {

                var exists = facultyDbContext.Courses.Any(s => s.CrsId == course.CrsId);

                if (exists)
                {
                    ModelState.AddModelError("CrsId", "Course ID already exists.");
                    ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");
                    return View(course);
                }


                if (course.DeptId.HasValue)
                {
                    var departmentExists = facultyDbContext.Departments.Any(d => d.DeptId == course.DeptId);
                    if (!departmentExists)
                    {
                        ModelState.AddModelError("DeptId", "The selected Department ID does not exist.");
                        ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");
                        return View(course);
                    }
                }


                facultyDbContext.Add(course);
                await facultyDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");
            return View(course);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var departments = facultyDbContext.Departments.ToList();
            ViewBag.DeptId = new SelectList(departments, "DeptId", "DeptName");
            var course = facultyDbContext.Courses.Where(c => c.CrsId == id).FirstOrDefault();
            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {

            if (course.DeptId.HasValue)
            {
                var departmentExists = facultyDbContext.Departments.Any(d => d.DeptId == course.DeptId);
                if (!departmentExists)
                {
                    ModelState.AddModelError("DeptId", "The selected Department ID does not exist.");
                    ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName", course.DeptId);
                    return View(course);
                }
            }

            if (ModelState.IsValid)
            {

                var currentCourse = facultyDbContext.Courses.FirstOrDefault(c => c.CrsId == course.CrsId);
                if (currentCourse == null)
                {
                    return NotFound();
                }


                currentCourse.CrsName = course.CrsName;
                currentCourse.CrsDuration = course.CrsDuration;
                currentCourse.DeptId = course.DeptId;

                facultyDbContext.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName", course.DeptId);
            return View(course);
        }



        public IActionResult Details(int id)
        {
            var course = facultyDbContext.Courses.Where(c => c.CrsId == id).Include(c => c.Dept).FirstOrDefault();
            var department = course.Dept;

            CourseVM courseVM = new CourseVM()
            {

                Name = course.CrsName
                ,
                Duration = course.CrsDuration,
                Id = course.CrsId,
                DeptID = course.DeptId,
            };
            return View(courseVM);
        }

        public IActionResult Delete(int id)
        {

            var currentCourse = facultyDbContext.Courses
                .Where(c => c.CrsId == id)
                .FirstOrDefault();

            if (currentCourse == null)
            {

                return NotFound();
            }


            var schedules = facultyDbContext.Schedules
                .Where(s => s.CrsId == id)
                .ToList();

            var course = facultyDbContext.Courses
                  .Include(d => d.Schedules)

                  .FirstOrDefault(d => d.CrsId == id);

            foreach (var schedule in course.Schedules)
            {
                schedule.CrsId = null;
            }

            facultyDbContext.Courses.Remove(currentCourse);
            facultyDbContext.SaveChanges();


            return RedirectToAction("Index");
        }


    }
}