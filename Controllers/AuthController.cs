using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using EscrowService.JWT;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // private readonly IJWTAUTH _jwtauth;

        public AuthController(IUserService userService)
        {
            _userService = userService;
            // _jwtauth = jwtauth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest _request)
        {
            var role = "";
            var login = await _userService.Login(_request);
            
            return Ok(login);
        }
    }
}