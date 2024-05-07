using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class QuizAttemptRepository(CoursifyContext context) : IQuizAttempRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> DeleteCollection(ICollection<QuizAttempt> quizAttempts)
        {
            _context.QuizAttempts.RemoveRange(quizAttempts);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Add(QuizAttempt quizAttempt)
        {
            await _context.QuizAttempts.AddAsync(quizAttempt);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
