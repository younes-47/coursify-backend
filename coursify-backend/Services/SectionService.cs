using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using coursify_backend.Repository;

namespace coursify_backend.Services
{
    public class SectionService(ISectionRepository sectionRepository,
        ICourseProgressRepository courseProgressRepository,
        CoursifyContext context,
        IUserRepository userRepository) : ISectionService
    {
        private readonly ISectionRepository _sectionRepository = sectionRepository;
        private readonly ICourseProgressRepository _courseProgressRepository = courseProgressRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly CoursifyContext _context = context;

        public async Task<bool> MarkCompleted(string email, int sectionId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int userId = await _userRepository.GetIdByEmail(email);
                CourseProgress? courseProgress = await _courseProgressRepository.GetBySectionIdAndUserId(sectionId, userId);
                if (courseProgress == null) return false;

                courseProgress.IsCompleted = true;
                await _courseProgressRepository.Update(courseProgress);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> MarkIncomplete(string email, int sectionId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int userId = await _userRepository.GetIdByEmail(email);
                CourseProgress? courseProgress = await _courseProgressRepository.GetBySectionIdAndUserId(sectionId, userId);
                if (courseProgress == null) return false;

                courseProgress.IsCompleted = false;
                await _courseProgressRepository.Update(courseProgress);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
