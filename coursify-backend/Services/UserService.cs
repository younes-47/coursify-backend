using AutoMapper;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Services
{
    public class UserService : IUserService
    {
        private readonly CoursifyContext _coursifyContext;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMiscService _miscService;
        private readonly IMapper _mapper;

        public UserService(CoursifyContext coursifyContext, 
            IUserRepository userRepository, 
            IAuthService authService,
            IMiscService miscService, 
            IMapper mapper)
        {
            _coursifyContext = coursifyContext;
            _userRepository = userRepository;
            _authService = authService;
            _miscService = miscService;
            _mapper = mapper;
        }

        public async Task<ProcessResult> RegisterNewUser(RegisterRequest registerRequest)
        {
            var transaction = _coursifyContext.Database.BeginTransaction();
            var result = new ProcessResult();
            try
            {
                User mappedUser = _mapper.Map<User>(registerRequest);
                mappedUser.Password = _authService.HashPassword(registerRequest.Password);
                mappedUser.EmailVerificationToken = _authService.CreateEmailVerficiationToken(registerRequest.Email);
                await _userRepository.Add(mappedUser);

                EmailDTO emailDTO = _miscService.GenerateVerificationEmail(mappedUser);
                result.Success = _miscService.SendEmail(emailDTO);
                if (!result.Success)
                {
                    transaction.Rollback();
                    result.Message = "Échec de l'envoi de l'e-mail de vérification";
                    return result;
                }
                await transaction.CommitAsync();
                result.Success = true;
                return result;

            }
            catch (Exception e)
            {
                transaction.Rollback();
                result.Success = false;
                result.Message = e.Message;
            }
            return result;
        }

        public async Task<ProcessResult> VerifyEmail(VerifyEmail verifyEmail)
        {
            var transaction = _coursifyContext.Database.BeginTransaction();
            var result = new ProcessResult();
            try
            {
                var user = await _userRepository.GetByEmailAsync(verifyEmail.Email);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "USER_NOT_FOUND";
                    return result;
                }
                if (user.EmailVerifiedAt != null)
                {
                    result.Success = false;
                    result.Message = "EMAIL_ALREADY_VERIFIED";
                    return result;
                }
                if (user.EmailVerificationToken != verifyEmail.Token)
                {
                    result.Success = false;
                    result.Message = "INVALID_TOKEN";
                    return result;
                }
                if (_authService.IsTokenExpired(verifyEmail.Token))
                {
                    result.Success = false;
                    result.Message = "TOKEN_EXPIRED";
                    return result;
                }
                
                user.EmailVerifiedAt = DateTime.Now;
                user.EmailVerificationToken = null;
                await _userRepository.Update(user);

                await transaction.CommitAsync();
                result.Success = true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                result.Success = false;
                result.Message = e.Message;
            }
            
            return result;
        }

        public async Task<ProcessResult> SendVerficationEmail(string email)
        {
            var result = new ProcessResult();
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                result.Success = false;
                result.Message = "UNKNOWN_EMAIL";
                return result;
            }
            if (user.EmailVerifiedAt != null)
            {
                result.Success = false;
                result.Message = "EMAIL_ALREADY_VERIFIED";
                return result;
            }
            if (user.EmailVerificationToken != null)
            {
                if (_authService.IsTokenExpired(user.EmailVerificationToken) == false)
                {
                    result.Success = false;
                    result.Message = "CURRENT_TOKEN_NOT_EXPIRED_YET";
                    return result;
                }
            }
            user.EmailVerificationToken = _authService.CreateEmailVerficiationToken(email);
            await _userRepository.Update(user);
            EmailDTO emailDTO = _miscService.GenerateVerificationEmail(user);
            result.Success = _miscService.SendEmail(emailDTO);
            if (!result.Success)
            {
                result.Message = "Échec de l'envoi de l'e-mail de vérification";
            }
            return result;
        }
    }
}
