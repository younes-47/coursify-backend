
namespace coursify_backend.DTO.GET
{
    public class EvaluationDetailsDTO
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public List<EvaluationQuestionsDTO> Questions { get; set; } = null!;

    }
}
