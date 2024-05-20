using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.GET
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public List<Byte[]> Slides { get; set; } = null!;

        public List<Byte[]> Documents { get; set; } = null!;

    }
}
