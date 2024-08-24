using System;
using System.ComponentModel.DataAnnotations;

namespace Faculty_M.Models;

public partial class Schedule
{
    [Required(ErrorMessage = "Schedule ID is required.")]
    public int SchId { get; set; }

    public int? CrsId { get; set; }

    public int? InsId { get; set; }

    [Required(ErrorMessage = "Day of the Week is required.")]
    [Display(Name = "Day of the Week")]
    [RegularExpression("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$",
        ErrorMessage = "Day of Week must be a valid day name.")]
    public string DayOfWeek { get; set; } = null!;

    [Required(ErrorMessage = "Start Time is required.")]
    [DataType(DataType.Time)]
    public TimeOnly? StartTime { get; set; }

    [Required(ErrorMessage = "End Time is required.")]
    [DataType(DataType.Time)]
    public TimeOnly? EndTime { get; set; }

    [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
    public string? Location { get; set; }


    public virtual Course? Crs { get; set; }

    public virtual Instructor? Ins { get; set; }
    public class TimeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var schedule = (Schedule)validationContext.ObjectInstance;

            if (schedule.StartTime.HasValue && schedule.EndTime.HasValue)
            {
                if (schedule.StartTime >= schedule.EndTime)
                {
                    return new ValidationResult("StartTime must be earlier than EndTime.");
                }
            }

            return ValidationResult.Success;
        }
    }
}