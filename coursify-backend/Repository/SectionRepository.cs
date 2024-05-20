using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class SectionRepository(CoursifyContext context) : ISectionRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> Add(Section section)
        {
            await _context.Sections.AddAsync(section);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExisted(int id)
        {
            return await _context.Sections.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> DeleteCollection(ICollection<Section> sections)
        {
            _context.Sections.RemoveRange(sections);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Section>> GetByCourseId(int courseId)
        {
            return await _context.Sections.Where(s => s.CourseId == courseId).ToListAsync();
        }

    }
}
