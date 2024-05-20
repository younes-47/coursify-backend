using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class CourseProgressRepository(CoursifyContext context) : ICourseProgressRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> Add(CourseProgress courseProgress)
        {
            await _context.CourseProgresses.AddAsync(courseProgress);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(CourseProgress courseProgress)
        {
            _context.CourseProgresses.Update(courseProgress);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<CourseProgress>> GetByCourseId(int courseId)
        {
            return await _context.CourseProgresses
                .Include(cp => cp.Section)
                .Where(cp => cp.Section.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<CourseProgress?> GetBySectionIdAndUserId(int sectionId, int userId)
        {
            return await _context.CourseProgresses
                .FirstOrDefaultAsync(cp => cp.SectionId == sectionId && cp.UserId == userId);
        }

        public async Task<bool> DeleteCollection(ICollection<CourseProgress> courseProgresses)
        {
            _context.CourseProgresses.RemoveRange(courseProgresses);
            return await _context.SaveChangesAsync() > 0;
        }

    }

}
