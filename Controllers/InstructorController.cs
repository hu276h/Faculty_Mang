using Faculty_M.Models;
using Faculty_M.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Faculty_M.Controllers
{
    public class InstructorController : Controller
    {
        private readonly FacultyDbContext facultyDbContext;
        public InstructorController(FacultyDbContext Context)
        {
            facultyDbContext = Context;
        }
        public IActionResult Index()
        {
            ICollection<Instructor> instructors = facultyDbContext.Instructors.ToList();
            return View(instructors);
        }

        public IActionResult Create()
        {

            var departments = facultyDbContext.Departments.ToList();
            ViewBag.DeptId = new SelectList(departments, "DeptId", "DeptName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {

                var exists = facultyDbContext.Instructors.Any(s => s.InsId == instructor.InsId);

                if (exists)
                {
                    ModelState.AddModelError("InsId", "Instructor ID already exists.");
                    ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");
                    return View(instructor);
                }


                if (instructor.DeptId.HasValue)
                {
                    var departmentExists = facultyDbContext.Departments.Any(d => d.DeptId == instructor.DeptId);

                    if (!departmentExists)
                    {
                        ModelState.AddModelError("DeptId", "The selected Department ID does not exist.");
                        ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");
                        return View(instructor);
                    }
                }


                facultyDbContext.Add(instructor);
                await facultyDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");
            return View(instructor);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var departments = facultyDbContext.Departments.ToList();
            ViewBag.DeptId = new SelectList(departments, "DeptId", "DeptName");

            var instructor = facultyDbContext.Instructors.FirstOrDefault(i => i.InsId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            var currentInstructor = facultyDbContext.Instructors.FirstOrDefault(i => i.InsId == instructor.InsId);
            if (currentInstructor == null)
            {
                return NotFound();
            }


            if (instructor.DeptId.HasValue)
            {
                var departmentExists = facultyDbContext.Departments.Any(d => d.DeptId == instructor.DeptId);
                if (!departmentExists)
                {
                    ModelState.AddModelError("DeptId", "The selected Department ID does not exist.");
                }
            }

            if (ModelState.IsValid)
            {

                currentInstructor.InsName = instructor.InsName;
                currentInstructor.InsDegree = instructor.InsDegree;
                currentInstructor.DeptId = instructor.DeptId;
                currentInstructor.Salary = instructor.Salary;
                currentInstructor.PhoneNumber = instructor.PhoneNumber;
                currentInstructor.Email = instructor.Email;

                try
                {

                    facultyDbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, "An error occurred while updating the instructor: " + ex.Message);
                }
            }


            ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName", instructor.DeptId);
            return View(instructor);
        }



        public IActionResult Details(int id)
        {
            var instructor = facultyDbContext.Instructors.Where(s => s.InsId == id).Include(s => s.Dept).FirstOrDefault();
            var department = instructor.Dept;

            InstructorVM instructorVM = new InstructorVM()
            {

                Name = instructor.InsName
                ,
                degree = instructor.InsDegree,
                Id = instructor.InsId,
                Email = instructor.Email,
                PhoneNumber = instructor.PhoneNumber,
                Salary = instructor.Salary,
                DeptID = instructor.DeptId,
                InstructorDepartment = department?.DeptName
            };
            return View(instructorVM);
        }

        public IActionResult Delete(int id)
        {

            var currentInstructor = facultyDbContext.Instructors
                .Where(i => i.InsId == id)
                .FirstOrDefault();

            if (currentInstructor == null)
            {

                return NotFound();
            }


            var instructor = facultyDbContext.Instructors
                 .Include(d => d.Schedules)
                
                 .FirstOrDefault(d => d.InsId == id);

            foreach (var schedule in instructor.Schedules)
            {
                schedule.InsId = null;
            }


            facultyDbContext.Instructors.Remove(currentInstructor);
            facultyDbContext.SaveChanges();


            return RedirectToAction("Index");
        }


    }
}