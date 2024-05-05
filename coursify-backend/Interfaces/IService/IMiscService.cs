using coursify_backend.DTO.INTERNAL;
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IService
{
    public interface IMiscService
    {
        EmailDTO GenerateVerificationEmail(User user);

        EmailDTO GeneratePasswordResetEmail(User user);

        bool SendEmail(EmailDTO emailDTO);

        bool DeleteWholeFolder(int courseId);
    }
}
