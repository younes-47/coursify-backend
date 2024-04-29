using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class CourseRepository(CoursifyContext context) : ICourseRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> Add(Course course)
        {
            await _context.Courses.AddAsync(course);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
