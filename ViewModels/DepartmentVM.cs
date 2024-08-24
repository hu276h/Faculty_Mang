namespace Faculty_M.ViewModels
{
    public class DepartmentVM
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Id { get; set; }
        public DateOnly? Date { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public List<string> Instructors { get; set; }
        public List<string> Students { get; set; }
    }

}
