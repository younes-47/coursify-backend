using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class SectionDTO
    {
        [Required(ErrorMessage = "Le titre d'une section est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Le titre d'une section doit comporter entre 1 et 255 caractères")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Les slides d'une section sont requis")]
        public List<Byte[]> Slides { get; set; } = null!;

        // Documents Not required
        public List<Byte[]> Documents { get; set; } = null!;

    }
}
