using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ICourseProgressRepository
    {
        Task<bool> Add(CourseProgress courseProgress);
        Task<bool> Update(CourseProgress courseProgress);
        Task<List<CourseProgress>> GetByCourseId(int courseId);
        Task<CourseProgress?> GetBySectionIdAndUserId(int sectionId, int userId);
        Task<bool> DeleteCollection(ICollection<CourseProgress> courseProgresses);

    }
}
