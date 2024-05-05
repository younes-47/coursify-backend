using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class EnrollementRepository(CoursifyContext context) : IEnrollementRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> DeleteCollection(ICollection<Enrollment> enrollements)
        {
            _context.Enrollments.RemoveRange(enrollements);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Add(Enrollment enrollement)
        {
            await _context.Enrollments.AddAsync(enrollement);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
