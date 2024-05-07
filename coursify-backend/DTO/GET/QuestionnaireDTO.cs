
namespace coursify_backend.DTO.GET
{
    public class QuestionnaireDTO
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public List<QuestionDetailsDTO> Questions { get; set; } = null!;

    }
}
