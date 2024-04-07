using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace coursify_backend.Controllers
{
    [Authorize]
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


        [AllowAnonymous]
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
            var authResponse = await _authService.AuthenticateAsync(authRequest);
            if (authResponse == null)
            {
                return Unauthorized("INVALID_PASSWORD");
            }

            return Ok(authResponse);
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("email/verification/verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmail verifyEmail)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.VerifyEmail(verifyEmail);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("email/verification/send")]
        public async Task<IActionResult> SendVerificationEmail([FromBody] string email)
        {

            bool isRegistered = await _userRepository.IsRegistered(email);
            if (!isRegistered) return BadRequest("UNREGISTRED_USER");

            ProcessResult result = await _userService.SendVerficationEmail(email);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }



    }
}
