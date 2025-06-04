using JWT_Authentication_API.Models.ViewModel;
using JWT_Authentication_API.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Authentication_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginVM loginVM)
        {
            var user = await _userService.Authenticate(loginVM);
            if (user == null) return BadRequest("Wrong User / Pwd");
            return Ok(user);

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            var user = await _userService.Register(registerVM);
            if (user == null) return BadRequest("User registration failed. Possible duplicate username or weak password.");
            return Ok(user);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserVM updateUserVM)
        {
            var updatedUser = await _userService.UpdateUser(id, updateUserVM);
            if (updatedUser == null) return NotFound("User not found or update failed.");
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var success = await _userService.DeleteUser(id);
            if (!success) return NotFound("User not found or delete failed.");
            return Ok("User deleted successfully.");
        }

    }
}
