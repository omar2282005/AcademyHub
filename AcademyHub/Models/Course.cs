using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyHub.Models
{
    public class Course
    {        

        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        [ForeignKey("Instructor")]
        public string InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public string CreatedByUserId { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();

    }
}