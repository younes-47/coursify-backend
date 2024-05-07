using coursify_backend.DTO.GET;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> IsExisted(int quizId)
        {
            return await _context.Quizzes.AnyAsync(e => e.Id == quizId);
        }

        public async Task<QuestionnaireDTO> GetByCourseId(int courseId)
        {
            return await _context.Quizzes
                .Include(e => e.Course)
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .Where(e => e.CourseId == courseId)
                .Select(e => new QuestionnaireDTO
                {
                    Id = e.Id,
                    CourseTitle = e.Course.Title,
                    Questions = e.Questions.Select(q => new QuestionDetailsDTO
                    {
                        QuestionId = q.Id,
                        Question = q.QuestionText,
                        Answers = q.Answers.Select(a => new AnswerDTO
                        {
                            Id = a.Id,
                            Answer = a.AnswerText,
                        }).ToList(),
                    }).ToList(),
                }).FirstAsync();
        }

        public async Task<int> GetQuestionsCount(int quizId)
        {
            return await _context.Quizzes
                .Include(e => e.Questions)
                .Where(e => e.Id == quizId)
                .Select(e => e.Questions.Count)
                .FirstAsync();
        }

        public async Task<int> GetCourseId(int quizId)
        {
            return await _context.Quizzes
                .Where(e => e.Id == quizId)
                .Select(e => e.CourseId)
                .FirstAsync();
        }

    }
}
