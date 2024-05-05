using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IAnswerRepository
    {
        Task<bool> Add(Answer answer);
        Task<bool> DeleteCollection(ICollection<Answer> answers);
    }
}
