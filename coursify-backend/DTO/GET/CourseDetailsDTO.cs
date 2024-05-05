namespace coursify_backend.DTO.GET
{
    public class CourseDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string? Description { get; set; }
        public Byte[]? Cover { get; set; } 
        public int TotalSections { get; set; }
        public int TotalSlides { get; set; }
        public int TotalDocuments { get; set; }
        public bool IsEnrolled { get; set; }

    }
}
