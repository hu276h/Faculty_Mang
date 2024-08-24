using Faculty_M.Models;
using Faculty_M.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Faculty_M.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly FacultyDbContext facultyDbContext;
        public DepartmentController(FacultyDbContext Context)
        {
            facultyDbContext = Context;
        }
        public IActionResult Index()
        {
            ICollection<Department> departments = facultyDbContext.Departments.ToList();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var exists = facultyDbContext.Departments.Any(s => s.DeptId == department.DeptId);
                if (exists)
                {
                    ModelState.AddModelError("DeptId", "Department ID already exists.");
                    return View(department);
                }

                facultyDbContext.Add(department);
                await facultyDbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(department);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = facultyDbContext.Departments.Where(c => c.DeptId == id).FirstOrDefault();
            return View(department);
        }
        [HttpPost]
        public IActionResult Edit(Department department)
        {

            var currentdepartment = facultyDbContext.Departments.FirstOrDefault(c => c.DeptId == department.DeptId);
            currentdepartment.DeptName = department.DeptName;
            currentdepartment.DeptLocation = department.DeptLocation;
            currentdepartment.DeptDesc = department.DeptDesc;
            currentdepartment.DeptManager = department.DeptManager;
            currentdepartment.ManagerHireDate = department.ManagerHireDate;
            facultyDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {

            var department = facultyDbContext.Departments
                            .Where(d => d.DeptId == id)
                            .Include(d => d.Instructors)
                            .Include(d => d.Students)
                            .FirstOrDefault();

            if (department == null)
            {
                return NotFound();
            }

            var departmentVM = new DepartmentVM
            {
                Name = department.DeptName,
                Desc = department.DeptDesc,
                Id = department.DeptId,
                Date = department.ManagerHireDate,
                Location = department.DeptLocation,
                Manager = department.DeptManager,
                Instructors = department.Instructors.Select(i => i.InsName).ToList(),
                Students = department.Students.Select(s => s.StName).ToList()
            };

            return View(departmentVM);
        }

        public IActionResult Delete(int id)
        {

            var department = facultyDbContext.Departments
                .Include(d => d.Students)
                .Include(d => d.Instructors)
                .Include(d => d.Courses)
                .FirstOrDefault(d => d.DeptId == id);

            if (department == null)
            {

                return NotFound();
            }


            foreach (var student in department.Students)
            {
                student.DeptId = null;
            }


            foreach (var instructor in department.Instructors)
            {
                instructor.DeptId = null;
            }


            foreach (var course in department.Courses)
            {
                course.DeptId = null;
            }


            facultyDbContext.SaveChanges();


            facultyDbContext.Departments.Remove(department);
            facultyDbContext.SaveChanges();


            return RedirectToAction("Index");
        }




    }
}