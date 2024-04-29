using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class EvaluationRepository(CoursifyContext coursifyContext) : IEvaluationRepository
    {
        private readonly CoursifyContext _coursifyContext = coursifyContext;

        public async Task<bool> Add(Evaluation evaluation)
        {
            await _coursifyContext.Evaluations.AddAsync(evaluation);
            return await _coursifyContext.SaveChangesAsync() > 0;
        }
    }
}
