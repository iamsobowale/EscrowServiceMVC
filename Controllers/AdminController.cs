using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody]CreateAdminRequestModel _request)
        {
            var response = await _adminService.CreateAdminAsync(_request);
            return Ok(response);
        }
        [HttpGet("GetAdminById")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var response = await _adminService.GetAdminAsync(id);
            return Ok(response);
        }
        [Authorize]
        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var response = await _adminService.GetAllAdminsAsync();
            return Ok(response);
        }
        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin(UpdateAdminRequestModel _request, int id)
        {
            var response = await _adminService.UpdateAdminAsync(_request, id);
            return Ok(response);
        }
        [HttpDelete("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var response = await _adminService.DeleteAdminAsync(id);
            return Ok(response);
        }
        [HttpGet("GetAdminByEmail")]
        public async Task<IActionResult> GetAdminByEmail(string email)
        {
            var response = await _adminService.GetAdminByEmailAsync(email);
            return Ok(response);
        }
        
    }
}