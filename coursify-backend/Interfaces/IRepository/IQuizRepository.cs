using coursify_backend.DTO.GET;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IQuizRepository
    {
        Task<bool> Add(Quiz quiz);
        Task<bool> Delete(Quiz quiz);
        Task<bool> IsExisted(int quizId);
        Task<QuestionnaireDTO> GetByCourseId(int courseId);
        Task<int> GetCourseId(int quizId);
        Task<int> GetQuestionsCount(int quizId);
    }
}
