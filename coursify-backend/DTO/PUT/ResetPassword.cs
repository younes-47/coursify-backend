using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.PUT
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Le mot de passe doit comporter entre 8 et 24 caractères")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Le token est requis")]
        public string Token { get; set; } = null!;
    }
}
