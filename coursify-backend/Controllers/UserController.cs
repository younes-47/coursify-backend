using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coursify_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            
        } 

        [Authorize(Roles = "user")]
        [HttpGet("test")]
        public async Task<IActionResult> RandomNumber()
        {
            return Ok(new {number = new Random().Next(1, 100)});
        }
    }
}
