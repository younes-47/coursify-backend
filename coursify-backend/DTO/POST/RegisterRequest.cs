using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.POST
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Le prénom est requis")]
        [MinLength(6, ErrorMessage = "Le prénom doit comporter au moins 2 caractères")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Le nom de famille est requis")]
        [MinLength(6, ErrorMessage = "Le nom de famille doit comporter au moins 2 caractères")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Le mot de passe doit comporter entre 8 et 24 caractères")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "La date de naissance est requise")]
        [CustomValidation(typeof(RegisterRequest), "ValidateDateOfBirth")]
        [DataType(DataType.Date)]
        public DateOnly Birthdate { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateOnly dateOfBirth, ValidationContext context)
        {
            var minDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-12)); // At least 12 years old
            if (dateOfBirth <= minDateOfBirth)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Vous devez avoir au moins 12 ans pour vous inscrire");
        }
    }
}
