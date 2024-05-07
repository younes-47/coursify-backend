using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Services
{
    public class QuizService(CoursifyContext context,
        IQuizRepository quizRepository,
        IUserRepository userRepository,
        IQuizAttempRepository quizAttempRepository,
        IQuestionRepository questionRepository) : IQuizService
    {
        private readonly CoursifyContext _context = context;
        private readonly IQuizRepository _quizRepository = quizRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IQuizAttempRepository _quizAttempRepository = quizAttempRepository;
        private readonly IQuestionRepository _questionRepository = questionRepository;

        public async Task<decimal> Pass(TestDTO testDTO, string email)
        {
            decimal quizResult = 0.0m;
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
                int questionsCount = await _quizRepository.GetQuestionsCount(testDTO.Id);
                quizResult = Math.Round(((decimal)correctAnswersCount / questionsCount) * 100, 2);
                int userId = await _userRepository.GetIdByEmail(email);

                QuizAttempt quizAttempt = new()
                {
                    QuizId = testDTO.Id,
                    UserId = userId,
                    Score = quizResult,
                };
                await _quizAttempRepository.Add(quizAttempt);

                await transaction.Result.CommitAsync();
            }
            catch (Exception)
            {

            }
            return quizResult;
        }

    }
}
