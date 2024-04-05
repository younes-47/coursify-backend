using coursify_backend.DTO.GET;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace coursify_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMiscService _miscService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IMiscService miscService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _miscService = miscService;
            _configuration = configuration;
        }
        public async Task<AuthResponse?> AuthenticateAsync(AuthRequest authRequest)
        {
           UserCredential userCredential = await _userRepository.GetCredentialByEmailAsync(authRequest.Email);

           if(!_miscService.VerifyPassword(authRequest.Password, userCredential.Password))
                return null;

            return CreateToken(userCredential);
        }

        public AuthResponse CreateToken(UserCredential userCredential)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, userCredential.Email),
                        new Claim(ClaimTypes.Role, userCredential.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponse { Token = tokenHandler.WriteToken(token), Role = userCredential.Role};
        }
    }
}
