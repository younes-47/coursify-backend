namespace coursify_backend.DTO.GET
{
    public class EnrolledCourseDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public Byte[]? Cover { get; set; }
        public decimal? HighestQuizScore { get; set; }
        public bool IsCompleted { get; set; }
        
        public decimal Progress { get; set; }
    }
}
