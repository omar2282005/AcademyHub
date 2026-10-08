namespace AcademyHub.ViewModel
{
    public class CourseIndexViewModel
    {
        public List<CourseListItemViewModel> Courses { get; set; }
            = new List<CourseListItemViewModel>();
        public CreateCourseViewModel NewCourse { get; set; }
             = new CreateCourseViewModel();

    }
}
