using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        [HttpPost("CreatePaymentMethod")]
        public async Task<IActionResult> CreatePaymentMethod(CreatePaymentMethodRequestModel createPaymentMethodRequestModel)
        {
            var result = await _paymentMethodService.CreatePaymentMethod(createPaymentMethodRequestModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("GetPaymentMethod")]
        public async Task<IActionResult> GetPaymentMethod(int id)
        {
            var result = await _paymentMethodService.GetPaymentMethod(id);
            if (result!=null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("GetAllPaymentMethod")]
        public async Task<IActionResult> GetAllPaymentMethod()
        {
            var result = await _paymentMethodService.GetAllPaymentMethod();
            if (result!=null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("UpdatePaymentMethod")]
        public async Task<IActionResult> UpdatePaymentMethod(UpdatePaymentMethodRequestModel updatePaymentMethodRequestModel, int id)
        {
            var result = await _paymentMethodService.UpdatePaymentMethod(updatePaymentMethodRequestModel, id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("DeletePaymentMethod")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            var result = await _paymentMethodService.DeletePaymentMethod(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpGet("GetPaymentMethodByName")]
        public async Task<IActionResult> GetPaymentMethodByName(string name)
        {
            var result = await _paymentMethodService.GetPaymentMethodByName(name);
            if (result!= null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}