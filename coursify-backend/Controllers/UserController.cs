﻿using AutoMapper;
using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
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
    public class UserController(IUserRepository userRepository,
        IUserService userService,
        ICourseRepository courseRepository,
        IEvaluationRepository evaluationRepository,
        IEvaluationService evaluationService,
        IMapper mapper) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly ICourseRepository _courseRepository = courseRepository;
        private readonly IEvaluationRepository _evaluationRepository = evaluationRepository;
        private readonly IEvaluationService _evaluationService = evaluationService;
        private readonly IMapper _mapper = mapper;

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

        [HttpGet("courses/available")]
        public async Task<IActionResult> GetAvailableCourses()
        {
            List<CourseDetailsDTO> courseInfos = await _courseRepository.GetAvailable(HttpContext.User.Identity.Name);
            return Ok(courseInfos);
        }

        [HttpGet("course/details")]
        public async Task<IActionResult> GetCourseDetails([FromQuery] int courseId)
        {
            if(!await _courseRepository.IsExisted(courseId))
                return NotFound("COURSE_NOT_FOUND");

            CourseDetailsDTO courseDetails = await _courseRepository.GetDetailsById(courseId, HttpContext.User.Identity.Name);
            return Ok(courseDetails);
        }

        [HttpGet("course/evaluation")]
        public async Task<IActionResult> GetCourseEvaluation([FromQuery] int courseId)
        {
            if(!await _courseRepository.IsExisted(courseId))
                return NotFound("COURSE_NOT_FOUND");

            EvaluationDetailsDTO evaluation = await _evaluationRepository.GetByCourseId(courseId);
            return Ok(evaluation);
        }

        [HttpPost("course/evaluate")]
        public async Task<IActionResult> GetCourseEvaluation([FromBody] TestDTO evaluation)
        {

            if(!await _evaluationRepository.IsExisted(evaluation.Id))
                return NotFound("EVALUATION_NOT_FOUND");

            TestResult testResult = await _evaluationService.Evaluate(evaluation, HttpContext.User.Identity.Name);
           
            return Ok(testResult);
        }

        [HttpGet("courses/enrolled")]
        public async Task<IActionResult> GetEnrolledCourses()
        {
            List<CourseDetailsDTO> courseInfos = await _courseRepository.GetEnrolled(HttpContext.User.Identity.Name);
            return Ok(courseInfos);
        }

    }
}
