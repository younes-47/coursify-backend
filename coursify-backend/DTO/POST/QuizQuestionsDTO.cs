using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class QuizQuestionsDTO
    {
        [Required(ErrorMessage = "Une question du quiz est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Une question du quiz doit comporter entre 1 et 255 caractères")]
        public string Question { get; set; } = null!;

        [Required(ErrorMessage = "Les réponses d'une question du quiz sont requis")]
        public AnswerDTO[] Answers { get; set; } = null!;

    }
}
