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
    }
}
