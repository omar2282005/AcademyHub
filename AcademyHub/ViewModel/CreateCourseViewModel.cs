using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace AcademyHub.ViewModel
{
    public class CreateCourseViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name ="Course Name")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Instructor")]
        public string InstructorId { get; set; }= string.Empty;
        public List<SelectListItem> Instructors { get; set; }
     = new List<SelectListItem>();
    }
}
