using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class EvaluationAttemptRepository(CoursifyContext context) : IEvaluationAttemptRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> DeleteCollection(ICollection<EvaluationAttempt> evaluationAttempts)
        {
            _context.EvaluationAttempts.RemoveRange(evaluationAttempts);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Add(EvaluationAttempt evaluationAttempt)
        {
            await _context.EvaluationAttempts.AddAsync(evaluationAttempt);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
