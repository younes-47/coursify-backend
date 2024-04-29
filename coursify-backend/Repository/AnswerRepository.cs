using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class AnswerRepository(CoursifyContext coursifyContext) : IAnswerRepository
    {
        private readonly CoursifyContext _coursifyContext = coursifyContext;

        public async Task<bool> Add(Answer answer)
        {
            await _coursifyContext.Answers.AddAsync(answer);
            return await _coursifyContext.SaveChangesAsync() > 0;
        }
    }
}
