namespace AcademyHub.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Enrollment> Enrollments  { get; set; }= new List<Enrollment>();

    }
}