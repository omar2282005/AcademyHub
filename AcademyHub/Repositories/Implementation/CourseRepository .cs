using AcademyHub.Data;
using AcademyHub.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AcademyHub.Repositories.Implementation
{
    public class CourseRepository : ICourseRepository
    {
        private readonly TrainingDbContext _context;
        public CourseRepository(TrainingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course)
        {
           await _context.Courses.AddAsync(course);
             
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses
                        .Include(c => c.Instructor)
                        .ToListAsync();
        }

        public Task<List<Instructor>> GetAllInstructorsAsync()
        {
            var ins = _context.Instructors.ToListAsync();
            return ins;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .ThenInclude(i => i.Profile)
                .Include(c =>c.Enrollments)
                
                .ThenInclude(e=>e.Student)
                .FirstOrDefaultAsync(c=>c.Id==id);
           
           return   course;
  
        }

        public Task<bool> InstructorExistsAsync(string instructorId)
        {
            var ins = _context.Instructors.AnyAsync(i=>i.Id==instructorId);
            
            return ins;

        }

        public void Remove(Course course)
        {
            _context.Courses.Remove(course);
        }

        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();

        }
    }
}
