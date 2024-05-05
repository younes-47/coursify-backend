using coursify_backend.DTO.INTERNAL;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ISlideRepository
    {
        Task<bool> Add(Slide slide);
        Task<bool> DeleteCollection(ICollection<Slide> slides);
    }
}
