using EscrowService.DTO;
using EscrowService.Interface.Service;
using EscrowService.JWT;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IJWTAUTH _jwtauth;
        // GET
        public IActionResult Login(UserLoginRequest _request)
        {
            return Ok();
        }
    }
}