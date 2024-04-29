using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ISectionRepository
    {
        Task<bool> Add(Section section);
    }
}
