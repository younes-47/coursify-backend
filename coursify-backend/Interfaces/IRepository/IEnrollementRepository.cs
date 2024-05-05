using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IEnrollementRepository
    {
        Task<bool> DeleteCollection(ICollection<Enrollment> enrollements);
        Task<bool> Add(Enrollment enrollement);
    }
}
