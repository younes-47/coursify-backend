using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;

namespace coursify_backend.Interfaces.IService
{
    public interface IAdminService
    {
        Task<ProcessResult> AddCourse(AddCourseDTO addCourseDTO);
    }
}
