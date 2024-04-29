using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ICourseRepository
    {
        Task<bool> Add(Course user);
    }
}
