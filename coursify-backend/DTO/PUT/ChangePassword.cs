using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.PUT
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Le mot de passe actuel est requis")]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Le mot de passe doit comporter entre 8 et 24 caractères")]
        public string NewPassword { get; set; } = "";
    }
}
