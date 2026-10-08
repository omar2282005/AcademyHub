namespace AcademyHub.ViewModel
{
    public class CourseListItemViewModel
    {
        public int CourseId { get; set; }
        public string InstructorName { get; set; }
        public string CourseName { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;

    }
}
