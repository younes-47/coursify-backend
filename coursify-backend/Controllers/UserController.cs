using AutoMapper;
using coursify_backend.DTO.GET;
using coursify_backend.DTO.PUT;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coursify_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IUserService userService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("Info")]
        public async Task<IActionResult> GetUserInfo()
        {
            User user = await _userRepository.GetByEmailAsync(HttpContext.User.Identity.Name);
            UserInfo profile = _mapper.Map<UserInfo>(user);
            return Ok(profile);
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile updateProfileRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            User user = await _userRepository.GetByEmailAsync(HttpContext.User.Identity.Name);
            var result = await _userService.UpdateProfile(user, updateProfileRequest);

            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePasswordRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            User user = await _userRepository.GetByEmailAsync(HttpContext.User.Identity.Name);
            var result = await _userService.ChangePassword(user, changePasswordRequest);

            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok();
        }

    }
}
