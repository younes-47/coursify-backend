using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IQuizRepository
    {
        Task<bool> Add(Quiz quiz);
        Task<bool> Delete(Quiz quiz);
    }
}
