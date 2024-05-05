using coursify_backend.DTO.GET;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IEvaluationRepository 
    {
        Task<bool> Add(Evaluation evaluation);
        Task<bool> Delete(Evaluation evaluation);
        Task<bool> IsExisted(int evaluationId);
        Task<EvaluationDetailsDTO> GetByCourseId(int courseId);
        Task<int> GetQuestionsCount(int evaluationId);
        Task<int> GetCourseId(int evaluationId);
    }
}
