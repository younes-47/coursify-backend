using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;

namespace coursify_backend.Interfaces.IService
{
    public interface IUserService
    {
        Task<ProcessResult> RegisterNewUser(RegisterRequest registerRequest);
        Task<ProcessResult> VerifyEmail(VerifyEmail verifyEmail);
        Task<ProcessResult> SendVerficationEmail(string email);
    }
}
