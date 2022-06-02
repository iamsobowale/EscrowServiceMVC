using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Service;
using EscrowService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EscrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ITraderService _traderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public TransactionController(ITransactionService transactionService, ITraderService traderService, IHttpContextAccessor contextAccessor)
        {
            _transactionService = transactionService;
            _traderService = traderService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody]CreateTransactionDto createTransactionDto)
        {
            var result = await _transactionService.CreateTransaction(createTransactionDto);
            if (result==null)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetTransaction")]
        public async Task<IActionResult> GetTransaction(int transactionId)
        {
            var result = await _transactionService.GetTransaction(transactionId);
            if (result == null)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetAllTransaction")]
        public async Task<IActionResult> GetAllTransaction()
        {
            var result = await _transactionService.GetAllTransaction();
            if (result.IsSuccess== false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetAllTransactionByReferenceId")]
        public async Task<IActionResult> GetAllTransactionByReferenceId(string referenceId)
        {
            var result = await _transactionService.GetTransactionByReferenceNumber(referenceId);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetAllTransactionByTraderEmail")]
        public async Task<IActionResult> GetAllTransactionByTraderEmail(string traderEmail)
        {
            var getTrader = await _traderService.GetTraderByEmailAsync(traderEmail);
            if (getTrader.IsSuccess==false)
            {
                return BadRequest(getTrader.Message);
            }
            var result = await _transactionService.GetAllTransactionsByTraderEmail(getTrader.Traders.Email);
            if (result.IsSuccess == false)
            {
                return Ok("Transaction not found");
            }
            return Ok(result);
        }
        [HttpGet("GetAllTransactionByTransactionStatus")]
        public async Task<IActionResult> GetAllTransactionByTransactionStatus(TransactionStatus transactionStatus, string email)
        {
            var result = await _transactionService.GetAllTransactionByTransactionStatus(transactionStatus, email);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("ConfirmTransaction")]
        public async Task<IActionResult> ConfirmTransaction(string reference)
        {
            var get =User.FindFirst(ClaimTypes.Name).Value;
            var getTrader = await _traderService.GetTraderByEmailAsync(get);
            if (getTrader.IsSuccess == false)
            {
                return BadRequest(getTrader.Message);
            }
            var result = await _transactionService.ConFirmTransaction(reference, getTrader.Traders.Email);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("CancelTransaction")]
        public async Task<IActionResult> CancelTransaction(string reference)
        {
            var get =User.FindFirst(ClaimTypes.Name).Value;
            var getTrader = await _traderService.GetTraderByEmailAsync(get);
            if (getTrader.IsSuccess==false)
            {
                return BadRequest(getTrader.Message);
            }

            var getTransaction = await _transactionService.CancelTransaction(reference, getTrader.Traders.Email);
            if (getTransaction.IsSuccess== false)
            {
                return BadRequest(getTransaction.Message);
            }

            return Ok(getTransaction);
        }
        [Authorize (Roles = "Trader")]
        [HttpPost("ProcessTrasaction")]
        public  async Task<IActionResult> ProcessTrasaction(string reference)
        {
            var get =User.FindFirst(ClaimTypes.Name).Value;
            var getTrader =await _traderService.GetTraderByEmailAsync(get);
            if (getTrader.IsSuccess==false)
            {
                return BadRequest(getTrader.Message);
            }
            var getTransaction = await _transactionService.ProcessTransaction(reference, getTrader.Traders.Email);
            if (getTransaction.IsSuccess== false)
            {
                return BadRequest(getTransaction.Message);
            }

            return Ok(getTransaction);
        }

        [HttpPost("GetInitiatedTransactionByTraderEmail")]
        public async Task<IActionResult> GetInitiatedTransactionByTraderEmail()
        { 
            var get =User.FindFirst(ClaimTypes.Name).Value;
            var getTransactionByTraderEmail = await _transactionService.GetInitiatedTransactionByTraderEmail(get);
            if (getTransactionByTraderEmail.IsSuccess==false)
            {
                return BadRequest(getTransactionByTraderEmail.Message);
            }
            return Ok(getTransactionByTraderEmail);
        }
        [HttpPost("MakeTransactionActive")]
        public async Task<IActionResult> MakeTransactionActive(string transactionNumber)
        {
            var get =User.FindFirst(ClaimTypes.Name).Value;
            var getTrader = await _traderService.GetTraderByEmailAsync(get);
            if (getTrader.IsSuccess==false)
            {
                return BadRequest(getTrader.Message);
            }
            var getTransaction = await _transactionService.MakeTransactionActive(transactionNumber, getTrader.Traders.Email);
            if (getTransaction.IsSuccess== false)
            {
                return BadRequest(getTransaction.Message);
            }

            return Ok(getTransaction);
        }

    }
}