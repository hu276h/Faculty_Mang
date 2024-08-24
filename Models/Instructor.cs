using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Faculty_M.Models;

public partial class Instructor
{
    [Required(ErrorMessage = "Id is reqired")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0 ")]

    public int InsId { get; set; }
    [Required(ErrorMessage = "Name is required")]

    public string? InsName { get; set; } = null!;
    [Required(ErrorMessage = "Degree is reqired")]

    public string? InsDegree { get; set; }
    [Required(ErrorMessage = "Salary is reqired")]

    public decimal? Salary { get; set; }

    [Required(AllowEmptyStrings = true)]
    [EmailAddress(ErrorMessage = "it doesn't match the email format")]

    public string? Email { get; set; }
    [Required]
    [RegularExpression(@"^(011|012|010|015)\d{8}$", ErrorMessage = "Phone number must start with 011, 012, 010, or 015 and be 11 digits long.")]
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    public int? DeptId { get; set; }
    public virtual Department? Dept { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}