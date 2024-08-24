using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Faculty_M.Models;

public partial class Department
{
    [Required(ErrorMessage = "Id is reqired")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0 ")]
    public int DeptId { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string DeptName { get; set; } = null!;
    [Required(ErrorMessage = "Department Description is required")]
    public string? DeptDesc { get; set; }
    [Required(ErrorMessage = "Department Location is required")]
    public string? DeptLocation { get; set; }
    [Required(ErrorMessage = "Department Manger is required")]
    public string? DeptManager { get; set; }
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Manager hire date is required.")]
    public DateOnly? ManagerHireDate { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}