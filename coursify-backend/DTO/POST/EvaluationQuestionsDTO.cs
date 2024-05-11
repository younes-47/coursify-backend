using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class EvaluationQuestionsDTO
    {
        [Required(ErrorMessage = "Une question de l'evaluation est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Une question de l'evaluation doit comporter entre 1 et 255 caractères")]
        public string Question { get; set; } = null!;

        [Required(ErrorMessage = "Les réponses d'une question de l'évaluation sont requis")]
        [CustomValidation(typeof(EvaluationQuestionsDTO), "AreAnswersValid")]
        public AnswerDTO[] Answers { get; set; } = null!;

        public static ValidationResult AreAnswersValid(AnswerDTO[] answers, ValidationContext context)
        {
            if (answers.Length != 4)
            {
                return new ValidationResult("Il doit y avoir exactement 4 réponses pour chaque question de l'évaluation");
            }
            if(answers.Count(a => a.IsCorrect) != 1)
            {
                return new ValidationResult("Il doit y avoir exactement une réponse correcte, parmi les réponses d'une question de l'évaluation");
            }
            return ValidationResult.Success;
        }
    }
}
