using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;

namespace EscrowService.Interface.Service
{
    public interface ITransactionService
    {
        public Task<BaseResponse> CreateTransaction(CreateTransactionDto transaction);
        public Task<TransactionResponseModel> GetTransaction(int transactionId);

        public Task<TransactionListResponseModel> GetAllTransaction();
        public Task<TransactionResponseModel> GetTransactionByReferenceNumber(string referenceNumber);
        public Task<TransactionListResponseModel> GetAllTransactionsByTraderEmail(string email);
        public Task<TransactionListResponseModel> GetAllTradersInTransaction(string referenceNumber);
        public Task<TransactionListResponseModel> GetAllTransactionByTransactionStatus(TransactionStatus status, string email);
        public Task<BaseResponse> ConFirmTransaction(string transactionId, string email);
        public Task<BaseResponse> CancelTransaction(string transactionId, string email);
        public Task<BaseResponse> ProcessTransaction(string transactionId, string email);

    }
}