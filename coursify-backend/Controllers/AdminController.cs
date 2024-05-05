using AutoMapper;
using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
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
        ICourseService adminService,
        ICourseRepository courseRepository,
        ICourseService courseService,
        ICategoryRepository categoryRepository) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly ICourseService _adminService = adminService;
        private readonly ICourseRepository _courseRepository = courseRepository;
        private readonly ICourseService _courseService = courseService;
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

        [HttpGet("course/all")]
        public async Task<IActionResult> GetAllCourses()
        {
            List<CourseAdminTableDTO> courses = await _courseRepository.GetAll();
            return Ok(courses);
        }

        [HttpDelete("course/delete")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            bool doesExist = await _courseRepository.IsExisted(courseId);
            if (!doesExist) return NotFound("COURSE_NOT_FOUND");

            ProcessResult result = await _courseService.DeleteCourse(courseId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Id);
        }

        [HttpGet("insights")]
        public async Task<IActionResult> GetInsights()
        {
            AdminInsightsDTO insights = new()
            {
                TotalUsers = await _userRepository.GetTotalUsers(),
                TotalCourses = await _courseRepository.GetTotal(),
                TotalAdmins = await _userRepository.GetTotalAdmins()
            };
            return Ok(insights);
        }
    }
}
