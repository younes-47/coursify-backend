namespace coursify_backend.DTO.GET
{
    public class TestDTO
    {
        public int Id { get; set; }

        public List<QuestionAnswerDTO> Answers { get; set; } = null!;
        
    }
}
