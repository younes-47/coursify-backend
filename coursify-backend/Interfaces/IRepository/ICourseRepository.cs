using coursify_backend.DTO.GET;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ICourseRepository
    {
        Task<bool> Add(Course user);
        Task<List<CourseAdminTableDTO>> GetAll();
        Task<Course> GetEverythingById(int id);
        Task<bool> IsExisted(int id);
        Task<bool> Delete(Course course);
        Task<int> GetTotal();
        Task<List<CourseDetailsDTO>> GetAvailable(string email);
        Task<CourseDetailsDTO> GetDetailsById(int id, string email);
        Task<List<CourseDetailsDTO>> GetEnrolled(string email);
    }
}
