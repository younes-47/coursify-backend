using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = "L'email est requis"), Required]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string Password { get; set; } = null!;
    }
}
