using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IEvaluationAttemptRepository
    {
        Task<bool> DeleteCollection(ICollection<EvaluationAttempt> evaluationAttempts);
        Task<bool> Add(EvaluationAttempt evaluationAttempt);
    }
}
