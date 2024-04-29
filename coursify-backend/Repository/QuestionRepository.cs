using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

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
    }
}
