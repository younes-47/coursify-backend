using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class AddCourseDTO
    {
        [Required(ErrorMessage = "Les informations d'une course sont requis")]
        public CourseInfoDTO CourseInfo { get; set; } = null!;

        [Required(ErrorMessage = "Les sections d'une course sont requis")]
        public SectionsDTO[] Sections { get; set; } = null!;

        [Required(ErrorMessage = "Les questions d'évaluation d'une course sont requis")]
        public EvaluationQuestionsDTO[] EvaluationQuestions { get; set; } = null!;

        [Required(ErrorMessage = "Les questions d'un quiz d'une course sont requis")]
        public QuizQuestionsDTO[] QuizQuestions { get; set; } = null!;
    }
}
