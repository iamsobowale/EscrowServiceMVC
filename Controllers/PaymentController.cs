using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentMethodService _paymentMethodService;
        // GET
        public PaymentController(ITransactionService transactionService, IPaymentService paymentService, IPaymentMethodService paymentMethodService)
        {
            _transactionService = transactionService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        [HttpPost("MakePayment")]
        public async Task<IActionResult> MakePayment(string transactionReference, string paymentMethod)
        {
            var paymentMethodId = await _paymentMethodService.GetPaymentMethodByName(paymentMethod);
            var transactionId = await _transactionService.GetTransactionByReferenceNumber(transactionReference);
            var paymentId = await _paymentService.CreatePayment(transactionId.Transaction.reference_id, paymentMethodId.PaymentMethod.PaymentMethodName);
            if (paymentId.IsSuccess == false)
            {
                return BadRequest(paymentId.Message);
            }
            return Ok(paymentId);
        }
    }
}