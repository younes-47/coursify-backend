namespace coursify_backend.DTO.GET
{
    public class QuestionDetailsDTO
    {
        public int QuestionId { get; set; }
        public string Question { get; set; } = null!;
        public List<AnswerDTO> Answers { get; set; } = null!;
    }
}
