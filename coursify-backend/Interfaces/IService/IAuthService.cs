using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;

namespace coursify_backend.Interfaces.IService
{
    public interface IAuthService
    {
        Task<AuthResponse?> AuthenticateAsync(LoginRequest authRequest);
        //Task<AuthResponse?> RefreshToken(string refreshToken);
        string CreateEmailVerficiationToken(string email);
        string CreatePasswordResetToken(string email);
        string HashPassword(string password);
        bool VerifyPassword(string inputPassword, string hashedPassword);
        bool IsTokenExpired(string token);
    }
}
