using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class QuestionRepository(CoursifyContext coursifyContext) : IQuestionRepository
    {
        private readonly CoursifyContext _coursifyContext = coursifyContext;

        public async Task<bool> Add(Question question)
        {
            await _coursifyContext.Questions.AddAsync(question);
            return await _coursifyContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCollection(ICollection<Question> questions)
        {
            _coursifyContext.Questions.RemoveRange(questions);
            return await _coursifyContext.SaveChangesAsync() > 0;         
        }

        public async Task<int> GetCorrectAnswerId(int questionId)
        {
            return await _coursifyContext.Answers
                .Where(a => a.QuestionId == questionId && a.IsCorrect == true)
                .Select(a => a.Id)
                .FirstAsync();
        }
    }
}
