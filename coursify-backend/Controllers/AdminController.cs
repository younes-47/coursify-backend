using AutoMapper;
using coursify_backend.DTO.GET;
using coursify_backend.DTO.POST;
using coursify_backend.DTO.PUT;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coursify_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController(
        IMapper mapper,
        IUserRepository userRepository,
        IUserService userService,
        IAdminService adminService,
        ICategoryRepository categoryRepository) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly IAdminService _adminService = adminService;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        [HttpGet("Info")]
        public async Task<IActionResult> GetAdminInfo()
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

        [HttpGet("course/categories")]
        public async Task<IActionResult> GetCategories()
        {
            List<CategoryDTO> categories = _mapper.Map<List<CategoryDTO>>(await _categoryRepository.GetAllCategories());
            return Ok(categories);
        }

        [HttpPost("course/add")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseDTO course)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _adminService.AddCourse(course);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
