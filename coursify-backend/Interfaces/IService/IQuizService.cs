using coursify_backend.DTO.GET;

namespace coursify_backend.Interfaces.IService
{
    public interface IQuizService
    {
        Task<decimal> Pass(TestDTO testDTO, string email);
    }
}
