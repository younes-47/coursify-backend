using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IQuestionRepository
    {
        Task<bool> Add(Question question);
        Task<bool> DeleteCollection(ICollection<Question> questions);
        Task<int> GetCorrectAnswerId(int questionId);
    }
}
