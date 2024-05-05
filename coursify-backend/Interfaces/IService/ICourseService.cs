using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;

namespace coursify_backend.Interfaces.IService
{
    public interface ICourseService
    {
        Task<ProcessResult> AddCourse(AddCourseDTO addCourseDTO);

        Task<ProcessResult> DeleteCourse(int courseId);
    }
}
