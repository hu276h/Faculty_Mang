using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Faculty_M.Models;

public partial class Student
{

    [Required(ErrorMessage = "Id is reqired")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0 ")]

    public int StId { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string StName { get; set; } = null!;

    [Required(ErrorMessage = "Address is required")]
    [MaxLength(50, ErrorMessage = "Address must be less than 50 character")]
    public string? StAddress { get; set; }
    [Required(ErrorMessage = "Age is required")]
    [Range(18, 25, ErrorMessage = "Age must be between 18 and 25")]
    public int? StAge { get; set; }

    public int? DeptId { get; set; }
    [Required(AllowEmptyStrings = true)]
    [EmailAddress(ErrorMessage = "it doesn't match the email format")]
    public string? Email { get; set; }
    [Required]
    [RegularExpression(@"^(011|012|010|015)\d{8}$", ErrorMessage = "Phone number must start with 011, 012, 010, or 015 and be 11 digits long.")]
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }
    [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0.")]
    public decimal? GPA { get; set; }
    public virtual Department? Dept { get; set; }
}