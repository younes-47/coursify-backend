namespace coursify_backend.Interfaces.IService
{
    public interface IMiscService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
