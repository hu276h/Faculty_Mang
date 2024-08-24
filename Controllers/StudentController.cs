using Faculty_M.Models;
using Faculty_M.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Faculty_M.Controllers
{
    public class StudentController : Controller
    {

        private readonly FacultyDbContext facultyDbContext;
        public StudentController(FacultyDbContext context)
        {
            facultyDbContext = context;
        }
        public IActionResult Index()
        {
            ICollection<Student> students = facultyDbContext.Students.ToList();

            return View(students);

        }
        public IActionResult Create()
        {
            var departments = facultyDbContext.Departments.ToList();
            ViewBag.DeptId = new SelectList(departments, "DeptId", "DeptName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {

            ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName");

            if (ModelState.IsValid)
            {

                if (student.DeptId.HasValue)
                {
                    var departmentExists = facultyDbContext.Departments.Any(d => d.DeptId == student.DeptId);
                    if (!departmentExists)
                    {
                        ModelState.AddModelError("DeptId", "The selected Department ID does not exist.");
                        return View(student);
                    }
                }


                var exists = facultyDbContext.Students.Any(s => s.StId == student.StId);
                if (exists)
                {
                    ModelState.AddModelError("StId", "Student ID already exists.");
                    return View(student);
                }


                facultyDbContext.Students.Add(student);
                await facultyDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(student);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var departments = facultyDbContext.Departments.ToList();
            ViewBag.DeptId = new SelectList(departments, "DeptId", "DeptName");
            var student = facultyDbContext.Students.Where(i => i.StId == id).FirstOrDefault();
            return View(student);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {

                if (student.DeptId.HasValue)
                {
                    var departmentExists = facultyDbContext.Departments.Any(d => d.DeptId == student.DeptId);
                    if (!departmentExists)
                    {
                        ModelState.AddModelError("DeptId", "The selected Department ID does not exist.");
                        ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName", student.DeptId);
                        return View(student);
                    }
                }


                var currentStudent = facultyDbContext.Students.FirstOrDefault(s => s.StId == student.StId);
                if (currentStudent == null)
                {
                    return NotFound();
                }


                currentStudent.StName = student.StName;
                currentStudent.DeptId = student.DeptId;
                currentStudent.StAge = student.StAge;
                currentStudent.Email = student.Email;
                currentStudent.PhoneNumber = student.PhoneNumber;
                currentStudent.StAddress = student.StAddress;
                currentStudent.GPA = student.GPA;

                facultyDbContext.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.DeptId = new SelectList(facultyDbContext.Departments.ToList(), "DeptId", "DeptName", student.DeptId);
            return View(student);
        }

        public IActionResult Details(int id)
        {
            var student = facultyDbContext.Students.Where(s => s.StId == id).Include(s => s.Dept).FirstOrDefault();
            var department = student.Dept;

            StudentVM studentVM = new StudentVM()
            {

                Name = student.StName
                ,
                Age = student.StAge,
                Id = student.StId,
                Address = student.StAddress,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                gpa = student.GPA,
                Student_Department = department?.DeptId
            };
            return View(studentVM);
        }
        public IActionResult Delete(int id)
        {
            var currentstudent = facultyDbContext.Students
                .Where(s => s.StId == id)
                .FirstOrDefault();
            facultyDbContext.Students.Remove(currentstudent);
            facultyDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}