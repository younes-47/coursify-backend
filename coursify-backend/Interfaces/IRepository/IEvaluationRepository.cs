using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IEvaluationRepository 
    {
        Task<bool> Add(Evaluation evaluation);
    }
}
