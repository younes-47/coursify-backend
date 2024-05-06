using coursify_backend.DTO.POST;

namespace coursify_backend.DTO.GET
{
    public class CourseContentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public List<SectionDTO> Sections { get; set; } = null!;
    }
}
