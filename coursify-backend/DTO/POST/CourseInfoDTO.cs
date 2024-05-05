using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class CourseInfoDTO
    {
        [Required(ErrorMessage = "Le titre de la course est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Le titre de la course doit comporter entre 1 et 255 caractères")]
        public string CourseTitle { get; set; } = null!;

        //[Required(ErrorMessage = "La description de la course est requis")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "La description de la course doit comporter entre 0 et 255 caractères")]
        public string CourseDescription { get; set; } = null!;

        [Required(ErrorMessage = "La categorie de la course est requis")]
        public int CourseCategoryId { get; set; }

        // Not required
        public Byte[]? CourseCover { get; set; }
    }
}
