using System.ComponentModel.DataAnnotations;

namespace coursify_backend.DTO.PUT
{
    public class UpdateProfile
    {

        [MinLength(2, ErrorMessage = "Le prénom doit comporter au moins 2 caractères")]
        public string FirstName { get; set; } = "";

        [MinLength(2, ErrorMessage = "Le nom de famille doit comporter au moins 2 caractères")]
        public string LastName { get; set; } = "";

        [CustomValidation(typeof(UpdateProfile), "ValidateDateOfBirth")]
        [DataType(DataType.Date)]
        public DateOnly Birthdate { get; set; }


        public string Avatar { get; set; } = "";


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
