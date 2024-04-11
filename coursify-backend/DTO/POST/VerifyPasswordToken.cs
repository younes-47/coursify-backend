using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class VerifyPasswordToken
    {
        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Le token est requis")]
        public string Token { get; set; } = null!;

    }
}
