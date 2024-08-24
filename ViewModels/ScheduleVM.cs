namespace Faculty_M.ViewModels
{
    public class ScheduleVM
    {

        public int ScheduleId { get; set; }
        public string? InsName { get; set; }
        public string? CrsName { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string Location { get; set; }

    }
}
