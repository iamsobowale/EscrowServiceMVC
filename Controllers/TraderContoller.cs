using System.Security.Claims;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraderContoller : ControllerBase
    {
        private readonly ITraderService _traderService;
        // GET
        public TraderContoller(ITraderService traderService)
        {
            _traderService = traderService;
        }
        [HttpPost("AddTrader")]
        public async Task<IActionResult> AddTrader([FromBody]CreateTraderRequestModel request)
        {
            var result = await _traderService.CreateTraderAsync(request);
            return Ok(result);
        }
        [HttpGet("GetTrader")]
        public async Task<IActionResult> GetTrader([FromQuery]int id)
        {
            var result = await _traderService.GetTraderAsync(id);
            return Ok(result);
        }
        [HttpGet("GetAllTrader")]
        public async Task<IActionResult> GetAllTrader()
        {
            var result = await _traderService.GetAllTradersAsync();
            return Ok(result);
        }
        [HttpPut("UpdateTrader")]
        public async Task<IActionResult> UpdateTrader(TraderUpdateRequestModel request)
        {
            var get =User.FindFirst(ClaimTypes.Name).Value;
            var result = await _traderService.UpdateTraderAsync(request, get);
            return Ok(result);
        }
        [HttpDelete("DeleteTrader")]
        public async Task<IActionResult> DeleteTrader(int id)
        {
            var result = await _traderService.DeleteTraderAsync(id);
            return Ok(result);
        }
        [HttpGet("GetTraderByEmail/{email}")]
        public async Task<IActionResult> GetTraderByEmail([FromRoute]string email)
        {
            var result = await _traderService.GetTraderByEmailAsync(email);
            return Ok(result);
        }
        [HttpGet("GetTraderByTransactionReference")]
        public async Task<IActionResult> GetTraderByTransactionReference(string transactionReference)
        {
            var result = await _traderService.GetAllTradersInTransaction(transactionReference);
            if (result.IsSuccess==false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
    }
}