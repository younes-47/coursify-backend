using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class EvaluationQuestionsDTO
    {
        [Required(ErrorMessage = "Une question de l'evaluation est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Une question de l'evaluation doit comporter entre 1 et 255 caractères")]
        public string Question { get; set; } = null!;

        [Required(ErrorMessage = "Les réponses d'une question de l'évaluation sont requis")]
        public AnswerDTO[] Answers { get; set; } = null!;
    }
}
