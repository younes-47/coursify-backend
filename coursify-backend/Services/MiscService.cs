using coursify_backend.DTO.INTERNAL;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Security.Cryptography;

namespace coursify_backend.Services
{
    public class MiscService : IMiscService
    {
        private readonly IConfiguration _configuration;

        public MiscService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EmailDTO GenerateVerificationEmail(User user)
        {
            string verificationLink = $"{_configuration["EmailSettings:CoursifyFrontendUrl"]}/Verify/token/{user.EmailVerificationToken}/email/{user.Email}";

            TextPart emailBody = new(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                    <h1>Veuillez confirmer votre adresse e-mail pour vous inscrire</h1>
                    <p>Merci d'avoir rejoint Coursify. Nous devons confirmer votre adresse e-mail. S'il vous plaît cliquer sur le lien ci-dessous. ce lien est valable 10 minutes</p>
                    <a href=""{verificationLink}"">Vérifier l'e-mail</a>
                "
            };

            EmailDTO emailDTO = new()
            {
                To = user.Email,
                Subject = "Vérifier votre e-mail",
                Body = emailBody
            };

            return emailDTO;
        }

        public EmailDTO GeneratePasswordResetEmail(User user)
        {
            string passwordResetLink = $"{_configuration["EmailSettings:CoursifyFrontendUrl"]}/Password-reset/token/{user.PasswordResetToken}/email/{user.Email}";

            TextPart emailBody = new(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                    <h1>Réinitialisation du mot de passe</h1>
                    <p>Vous avez demandé une réinitialisation de mot de passe. S'il vous plaît cliquer sur le lien ci-dessous pour réinitialiser votre mot de passe. ce lien est valable 10 minutes</p>
                    <a href=""{passwordResetLink}"">Réinitialiser</a>
                "
            };

            EmailDTO emailDTO = new()
            {
                To = user.Email,
                Subject = "Réinitialiser votre mot de passe",
                Body = emailBody
            };

            return emailDTO;
        }

        public bool SendEmail(EmailDTO emailDTO)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Coursify", _configuration["EmailSettings:EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(emailDTO.To));
                email.Subject = emailDTO.Subject;
                email.Body = emailDTO.Body;

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["EmailSettings:EmailUsername"], _configuration["EmailSettings:EmailPassword"]);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
