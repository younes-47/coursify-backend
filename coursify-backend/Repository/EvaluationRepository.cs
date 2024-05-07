using coursify_backend.DTO.GET;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class EvaluationRepository(CoursifyContext coursifyContext) : IEvaluationRepository
    {
        private readonly CoursifyContext _context = coursifyContext;

        public async Task<bool> Add(Evaluation evaluation)
        {
            await _context.Evaluations.AddAsync(evaluation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Evaluation evaluation)
        {
            _context.Evaluations.Remove(evaluation);
            return await _context.SaveChangesAsync() > 0;       
        }

        public async Task<bool> IsExisted(int evaluationId)
        {
            return await _context.Evaluations.AnyAsync(e => e.Id == evaluationId);
        }

        public async Task<QuestionnaireDTO> GetByCourseId(int courseId)
        {
            return await _context.Evaluations
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

        public async Task<int> GetQuestionsCount(int evaluationId)
        {
            return await _context.Evaluations
                .Include(e => e.Questions)
                .Where(e => e.Id == evaluationId)
                .Select(e => e.Questions.Count)
                .FirstAsync();
        }

        public async Task<int> GetCourseId(int evaluationId)
        {
            return await _context.Evaluations
                .Where(e => e.Id == evaluationId)
                .Select(e => e.CourseId)
                .FirstAsync();
        }
    }
}
