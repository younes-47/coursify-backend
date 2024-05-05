using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

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

        public async Task<bool> DeleteCollection(ICollection<Section> sections)
        {
            _context.Sections.RemoveRange(sections);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
