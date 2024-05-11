using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class AnswerDTO
    {
        [Required(ErrorMessage = "Une réponse parmi les 4 réponses d'une question de l'evaluation est requis")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Une réponse parmi les 4 réponses d'une question de l'evaluation doit comporter entre 1 et 255 caractères")]
        public string Answer { get; set; } = null!;

        [Required(ErrorMessage = "Vous deverez spécidier si une réponse d'une question est juste ou non!")]
        public bool IsCorrect { get; set; }
    }
}
