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
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IMiscService miscService, IUserRepository userRepository)
        {
            _authService = authService;
            _miscService = miscService;
            _userRepository = userRepository;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            if(!await _userRepository.IsRegistered(authRequest.Email))
            {
                return NotFound("L'adresse e-mail que vous avez saisie n'est connectée à aucun compte.");
            }
            var authResponse = await _authService.AuthenticateAsync(authRequest);
            if (authResponse == null)
            {
                return Unauthorized("Mot de passse incorrect.");
            }

            return Ok(authResponse);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _userRepository.IsRegistered(registerRequest.Email))
            {
                return Conflict("Un utilisateur avec cet email existe déjà");
            }
            //string hashedPassword = _miscService.HashPassword(registerRequest.Password);
            //await _userRepository.AddUserAsync(registerRequest.Email, hashedPassword, registerRequest.Role);
            return Ok();
        }



    }
}
