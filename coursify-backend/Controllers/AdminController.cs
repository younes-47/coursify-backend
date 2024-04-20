using AutoMapper;
using coursify_backend.DTO.GET;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coursify_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AdminController(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("Info")]
        public async Task<IActionResult> GetAdminInfo()
        {
            User user = await _userRepository.GetByEmailAsync(HttpContext.User.Identity.Name);
            UserInfo profile = _mapper.Map<UserInfo>(user);
            return Ok(profile);
        }
    }
}
