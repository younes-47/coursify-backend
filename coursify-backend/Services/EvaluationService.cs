using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using coursify_backend.Repository;

namespace coursify_backend.Services
{
    public class EvaluationService(CoursifyContext context,
        IEvaluationRepository evaluationRepository,
        IUserRepository userRepository,
        IEvaluationAttemptRepository evaluationAttemptRepository,
        IEnrollementRepository enrollementRepository,
        IQuestionRepository questionRepository) : IEvaluationService
    {
        private readonly CoursifyContext _context = context;
        private readonly IEvaluationRepository _evaluationRepository = evaluationRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEvaluationAttemptRepository _evaluationAttemptRepository = evaluationAttemptRepository;
        private readonly IEnrollementRepository _enrollementRepository = enrollementRepository;
        private readonly IQuestionRepository _questionRepository = questionRepository;

        public async Task<TestResult> Evaluate(TestDTO testDTO, string email)
        {
            TestResult testResult = new();
            var transaction = _context.Database.BeginTransactionAsync();
            try
            {
                int correctAnswersCount = 0;
                foreach (QuestionAnswerDTO questionAnswerDTO in testDTO.Answers)
                {
                    int correctAnswerId = await _questionRepository.GetCorrectAnswerId(questionAnswerDTO.QuestionId);
                    if (correctAnswerId == questionAnswerDTO.AnswerId)
                    {
                        correctAnswersCount++;
                    }
                }
                int questionsCount = await _evaluationRepository.GetQuestionsCount(testDTO.Id);
                decimal evaluationResult = Math.Round(((decimal)correctAnswersCount / questionsCount) * 100, 2);
                int userId = await _userRepository.GetIdByEmail(email);

                EvaluationAttempt evaluationAttempt = new()
                {
                    EvaluationId = testDTO.Id,
                    UserId = userId,
                    Score = evaluationResult,
                    IsPassed = evaluationResult >= 60,
                };
                await _evaluationAttemptRepository.Add(evaluationAttempt);

                testResult.Score = evaluationResult;
                testResult.IsPassed = evaluationResult >= 60;

                if(testResult.IsPassed)
                {
                    Enrollment enrollment = new()
                    {
                        CourseId = await _evaluationRepository.GetCourseId(testDTO.Id),
                        UserId = userId,
                    };
                    await _enrollementRepository.Add(enrollment);
                }

                await transaction.Result.CommitAsync();
            }
            catch (Exception)
            {
                
            }
            return testResult;
        }
    }
}
