using coursify_backend.DTO.INTERNAL;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IService
{
    public interface IMiscService
    {
        EmailDTO GenerateVerificationEmail(User user);

        EmailDTO GeneratePasswordResetEmail(User user);
        Task<bool> SendEmailAzure(EmailDTO emailDTO);
        bool DeleteWholeFolder(int courseId);
    }
}
