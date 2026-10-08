namespace AcademyHub.ViewModel
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }
        public string? InstructorBio { get; set; }
        public int StudentCount {  get; set; }
        public List<EnrolledStudentViewModel> Students { get; set; }= new List<EnrolledStudentViewModel>();
      
    }
}
