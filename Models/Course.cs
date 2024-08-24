using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Faculty_M.Models;

public partial class Course
{
    [Required(ErrorMessage = "Id is reqired")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0 ")]
    public int CrsId { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string? CrsName { get; set; }
    [Required]
    public int? CrsDuration { get; set; }
    public int? DeptId { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}