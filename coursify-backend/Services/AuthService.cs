using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace coursify_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        private const int SaltSize = 128 / 8; // 16 bytes
        private const int KeySize = 256 / 8; // 32 bytes
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize); // RandomNumberGenerator.Create()
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string[] chunks = hashedPassword.Split(Delimiter);
            byte[] salt = Convert.FromBase64String(chunks[0]);
            byte[] actualHash = Convert.FromBase64String(chunks[1]);

            byte[] hashedInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithm, KeySize);

            return CryptographicOperations.FixedTimeEquals(actualHash, hashedInput);
        }

        public async Task<AuthResponse?> AuthenticateAsync(LoginRequest authRequest)
        {
            try
            {
                User user = await _userRepository.GetByEmailAsync(authRequest.Email);

                if (!VerifyPassword(authRequest.Password, user.Password))
                    return null;

                AuthResponse authResponse = new()
                {
                    AccessToken = CreateAccessToken(user),
                    //RefreshToken = CreateRefreshToken(user),
                    Role = user.Role
                };

                //user.RefreshToken = authResponse.RefreshToken;
                //await _userRepository.Update(user);
                return authResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public async Task<AuthResponse?> RefreshToken(string refreshToken)
        //{
        //    try
        //    {
        //        User? user = await _userRepository.GetByRefreshToken(refreshToken);

        //        if (user == null)
        //            return null;

        //        if (user.RefreshToken != null && IsTokenExpired(user.RefreshToken))
        //        {
        //            user.RefreshToken = null;
        //            await _userRepository.Update(user);
        //            return null;
        //        }
                    
        //        if(user.RefreshToken != refreshToken)
        //            return null;


        //        AuthResponse authResponse = new()
        //        {
        //            AccessToken = CreateAccessToken(user),
        //            Role = user.Role
        //        };

        //        return authResponse;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public string CreateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return  tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddSeconds(40),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string CreateEmailVerficiationToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.Role, "user")
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string CreatePasswordResetToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.Role, "user")
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = false,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = _configuration["JWT:Audience"]
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return true;
            }
            return false;
        }

        
    }
}
