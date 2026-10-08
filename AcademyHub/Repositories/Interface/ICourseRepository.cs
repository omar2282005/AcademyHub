namespace AcademyHub.Repositories.Interface
{
    public interface ICourseRepository
    {
        public Task<List<Course>> GetAllAsync();
        public Task<Course?> GetByIdAsync(int id);
        public Task AddAsync(Course course);
        public Task SaveChangesAsync();
        public Task<List<Instructor>> GetAllInstructorsAsync();

        public  Task<bool> InstructorExistsAsync(string instructorId);
        public void Remove(Course course);
    }
}
