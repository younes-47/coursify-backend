using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class QuizRepository(CoursifyContext context) : IQuizRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> Add(Quiz quiz)
        {
            await _context.Quizzes.AddAsync(quiz);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Quiz quiz)
        {
            _context.Quizzes.Remove(quiz);
            return await _context.SaveChangesAsync() > 0;   
        }
    }
}
