namespace coursify_backend.DTO.GET
{
    public class UserInfo
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public string Avatar { get; set; } = null!;

        public string Email { get; set; } = null!;

    }
}
