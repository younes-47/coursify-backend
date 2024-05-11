using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.DTO.PUT;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace coursify_backend.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMiscService _miscService;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, 
            IMiscService miscService, 
            IUserService userService,
            IUserRepository userRepository)
        {
            _authService = authService;
            _miscService = miscService;
            _userService = userService;
            _userRepository = userRepository;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest authRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            if(!await _userRepository.IsRegistered(authRequest.Email))
            {
                return NotFound("UNKNOWN_EMAIL");
            }
            if(!await _userRepository.IsEmailVerified(authRequest.Email))
            {
                return Unauthorized("UNVERIFIED_EMAIL");
            }
            AuthResponse? authResponse = await _authService.AuthenticateAsync(authRequest);
            if (authResponse == null)
            {
                return Unauthorized("INVALID_PASSWORD");
            }

            // Set the refresh token in an http-only cookie
            HttpContext.Response.Cookies.Append("refreshToken", authResponse.RefreshToken, new CookieOptions
            {
                
                Expires = DateTime.Now.AddDays(1),
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
            });

            return Ok(new {authResponse.AccessToken, authResponse.Role});
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string? refreshToken = HttpContext.Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return NoContent();

            User? user = await _userRepository.GetByRefreshToken(refreshToken);
            if (user == null)
            {
                HttpContext.Response.Cookies.Delete("refreshToken");
                return NoContent();
            }

            user.RefreshToken = null;
            await _userRepository.Update(user);
            HttpContext.Response.Cookies.Delete("refreshToken");

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            string? refreshToken = HttpContext.Request.Cookies["refreshToken"];
            if (refreshToken == null)
            {
                return BadRequest("REFRESH_TOKEN_NOT_FOUND");
            }

            AuthResponse? authResponse = await _authService.RefreshToken(refreshToken);
            if (authResponse == null)
            {
                return BadRequest("INVALID_REFRESH_TOKEN");
            }

            return Ok(new { authResponse.AccessToken, authResponse.Role });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _userRepository.IsRegistered(registerRequest.Email))
                return Conflict("Un utilisateur avec cet email existe déjà");

            var result = await _userService.RegisterNewUser(registerRequest);
            if (!result.Success) return BadRequest(result.Message);
       
            return Ok();
        }

        [HttpPost("email/verification/verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailToken verifyEmail)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.VerifyEmail(verifyEmail);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPost("email/verification/send")]
        public async Task<IActionResult> SendVerificationEmail([FromBody] string email)
        {

            bool isRegistered = await _userRepository.IsRegistered(email);
            if (!isRegistered) return BadRequest("UNKNOWN_EMAIL");

            ProcessResult result = await _userService.SendVerficationEmail(email);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPost("email/password-reset/send")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] string email)
        {
            bool isRegistered = await _userRepository.IsRegistered(email);
            if (!isRegistered) return BadRequest("UNKNOWN_EMAIL");

            ProcessResult result = await _userService.SendPasswordResetEmail(email);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPost("Password-reset/verify-token")]
        public async Task<IActionResult> VerifyPasswordResetToken([FromBody] VerifyPasswordToken verifypasswordReset)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.VerifyPasswordResetToken(verifypasswordReset);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPost("Password-reset/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.ResetPassword(resetPassword);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }



    }
}
