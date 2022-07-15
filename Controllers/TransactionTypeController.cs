using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : Controller
    {
        private readonly ITranscationTypeService _transactionService;
        // GET
        
        public TransactionTypeController(ITranscationTypeService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> AddTransactions([FromBody]CreateTransactionTypeServiceDto createTransactionTypeServiceDto)
        {
            var result = await _transactionService.CreateTransactionType(createTransactionTypeServiceDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetAllTransactionTypeByTransactionReferenceNumber/{TransactionRefernceNumber}")]
        public async Task<IActionResult> GetAllTransactionTypeByTransactionReferenceNumber([FromRoute]string TransactionRefernceNumber)
        {
            var result = await _transactionService.GetAllTransactionTypeByReferenceNumber(TransactionRefernceNumber);
            if (result.IsSuccess==false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("AcceptTransaction/{transactionReferenceNumber}")]
        public async Task<IActionResult> AcceptTransaction([FromRoute]string transactionReferenceNumber)
        {
            var result = await _transactionService.AcceptSubTransaction(transactionReferenceNumber);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("RejectTransaction/{transactionReferenceNumber}")]
        public async Task<IActionResult> RejectTransaction([FromRoute]string transactionReferenceNumber)
        {
            var result = await _transactionService.RejectSubTransaction(transactionReferenceNumber);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetDeliveredSubTransaction/{TransactionReferenceNumber}")]
        public async Task<IActionResult> GetDeliveredSubTransaction([FromRoute]string transactionReferenceNumber)
        {
            var result = await _transactionService.GetDeliverSubTransaction(transactionReferenceNumber);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetAcceptedSubTransaction")]
        public async Task<IActionResult> GetDeliveredSubTransaction()
        {
            var result = await _transactionService.GetAcceptedSubTransaction();
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("MakeSubTransactionDone/{transactionReferenceNumber}")]
        public async Task<IActionResult> MakeSubTransactionDone([FromRoute]string transactionReferenceNumber)
        {
            var result = await _transactionService.MakeSubTransactionDone(transactionReferenceNumber);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetSubTransactionByTransactionRef/{transactionReferenceNumber}")]
        public async Task<IActionResult> GetSubTransactionByTransactionRef([FromRoute]string transactionReferenceNumber)
        {
            var result = await _transactionService.GetSubTransactionByTransactionRef(transactionReferenceNumber);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}