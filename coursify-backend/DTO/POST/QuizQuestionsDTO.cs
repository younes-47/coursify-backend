using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class QuizQuestionsDTO
    {
        [Required(ErrorMessage = "Une question du quiz est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Une question du quiz doit comporter entre 1 et 255 caractères")]
        public string Question { get; set; } = null!;

        [Required(ErrorMessage = "Les réponses d'une question du quiz sont requis")]
        [CustomValidation(typeof(QuizQuestionsDTO), "AreAnswersValid")]
        public AnswerDTO[] Answers { get; set; } = null!;

        public static ValidationResult AreAnswersValid(AnswerDTO[] answers, ValidationContext context)
        {
            if (answers.Length != 4)
            {
                return new ValidationResult("Il doit y avoir exactement 4 réponses pour chaque question du quiz");
            }
            if (answers.Any(a => a.IsCorrect))
            {
                return ValidationResult.Success;
            }        
            return new ValidationResult("Il doit y avoir exactement une réponse correcte, parmi les réponses d'une question du quiz");
        }

    }
}
