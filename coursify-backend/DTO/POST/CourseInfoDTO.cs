using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class CourseInfoDTO
    {
        [Required(ErrorMessage = "Le titre de la course est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Le titre de la course doit comporter entre 1 et 255 caractères")]
        public string CourseTitle { get; set; } = null!;

        //[Required(ErrorMessage = "La description de la course est requis")]
        [StringLength(2000, MinimumLength = 0, ErrorMessage = "La description de la course doit comporter entre 0 et 2000 caractères")]
        public string CourseDescription { get; set; } = null!;

        [Required(ErrorMessage = "La categorie de la course est requis")]
        public int CourseCategoryId { get; set; }

        // Not required
        [CustomValidation(typeof(CourseInfoDTO), "IsPng")]
        public Byte[]? CourseCover { get; set; }

        public static ValidationResult IsPng(Byte[]? CourseCover, ValidationContext context)
        {
            if (CourseCover == null)
            {
                return ValidationResult.Success;
            }
            if (CourseCover[0] != 0x89 || CourseCover[1] != 0x50 || CourseCover[2] != 0x4E || CourseCover[3] != 0x47 || CourseCover[4] != 0x0D || CourseCover[5] != 0x0A || CourseCover[6] != 0x1A || CourseCover[7] != 0x0A)
            {
                return new ValidationResult("La couverture de la course doit être une image PNG");
            }
            return ValidationResult.Success;    
        }
    }
}
