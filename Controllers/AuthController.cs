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
        private readonly IJWTAUTH _jwtauth;

        public AuthController(IUserService userService, IJWTAUTH jwtauth)
        {
            _userService = userService;
            _jwtauth = jwtauth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest _request)
        {
            var login = await _userService.Login(_request);
            if (login==null)
            {
                return BadRequest("Invalid Username or Password");
            }

            var response = new UserLoginResponse()
            {
                Email = _request.Email,
                Data = _jwtauth.GenerateToken(login.Data)
            };
            return Ok(response);
        }
    }
}