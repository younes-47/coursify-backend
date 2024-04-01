using coursify_backend.DTO.GET;
using coursify_backend.DTO.POST;

namespace coursify_backend.Interfaces.IService
{
    public interface IAuthService
    {
        Task<AuthResponse?> AuthenticateAsync(AuthRequest authRequest);
    }
}
