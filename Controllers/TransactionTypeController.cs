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
        public async Task<IActionResult> AddTransactions(IList<CreateTransactionTypeServiceDto> createTransactionTypeServiceDto, string TransactionType)
        {
            var result = await _transactionService.CreateTransactionType(createTransactionTypeServiceDto, TransactionType);
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
    }
}