using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class SectionDTO
    {
        [Required(ErrorMessage = "Le titre d'une section est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Le titre d'une section doit comporter entre 1 et 255 caractères")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Les slides d'une section sont requis")]
        [CustomValidation(typeof(SectionDTO), "IsPng")]
        public List<Byte[]> Slides { get; set; } = null!;

        // Documents Not required
        [CustomValidation(typeof(SectionDTO), "IsPdf")]
        public List<Byte[]> Documents { get; set; } = null!;

        public static ValidationResult IsPng(List<Byte[]> Slides, ValidationContext context)
        {
            foreach (var slide in Slides)
            {
                if (slide.Length < 8 || slide[0] != 137 || slide[1] != 80 || slide[2] != 78 || slide[3] != 71 || slide[4] != 13 || slide[5] != 10 || slide[6] != 26 || slide[7] != 10)
                {
                    return new ValidationResult("Les slides doivent être des images PNG");
                }
            }
            return ValidationResult.Success;
          
        }

        public static ValidationResult IsPdf(List<Byte[]> Documents, ValidationContext context)
        {
            foreach (var document in Documents)
            {
                if (document.Length < 4 || document[0] != 37 || document[1] != 80 || document[2] != 68 || document[3] != 70)
                {
                    return new ValidationResult("Les documents doivent être des fichiers PDF");
                }
            }
            return ValidationResult.Success;
        }

    }
}
