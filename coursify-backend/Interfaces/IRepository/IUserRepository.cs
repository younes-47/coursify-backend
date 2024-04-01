using coursify_backend.DTO.GET;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<UserCredential> GetCredentialByEmailAsync(string email);
        Task<bool> IsRegistered(string email);
    }
}
