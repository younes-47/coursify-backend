using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;

namespace coursify_backend.Interfaces.IService
{
    public interface IEvaluationService
    {
        Task<TestResult> Evaluate(TestDTO testDTO, string email);
    }
}
