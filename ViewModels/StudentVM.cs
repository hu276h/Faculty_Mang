namespace Faculty_M.ViewModels
{
    public class StudentVM
    {
        public int Id { get; set; }
        public int? Student_Department { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal? gpa { get; set; }
    }
}
