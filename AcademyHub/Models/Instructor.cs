namespace AcademyHub.Models
{
    public class Instructor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Course> Courses { get; set; }= new List<Course>();
        public InstructorProfile? Profile { get; set; }
    }
}