using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyHub.Models
{
    public class InstructorProfile
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        [ForeignKey("Instructor")]
        public string InstructorId { get; set; }
        public Instructor Instructor { get; set; }

    }
}