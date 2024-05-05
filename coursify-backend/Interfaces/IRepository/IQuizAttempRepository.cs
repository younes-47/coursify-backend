using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IQuizAttempRepository
    {
        Task<bool> DeleteCollection(ICollection<QuizAttempt> quizAttempts);
    }
}
