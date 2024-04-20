using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.DTO.PUT;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IService
{
    public interface IUserService
    {
        Task<ProcessResult> RegisterNewUser(RegisterRequest registerRequest);
        Task<ProcessResult> VerifyEmail(VerifyEmailToken verifyEmail);
        Task<ProcessResult> VerifyPasswordResetToken(VerifyPasswordToken verifypasswordReset);
        Task<ProcessResult> SendVerficationEmail(string email);
        Task<ProcessResult> SendPasswordResetEmail(string email);
        Task<ProcessResult> ResetPassword(ResetPassword resetPassword);
        Task<ProcessResult> UpdateProfile(User user, UpdateProfile updateProfileRequest);

        Task<ProcessResult> ChangePassword(User user, ChangePassword changePasswordRequest);
    }
}
