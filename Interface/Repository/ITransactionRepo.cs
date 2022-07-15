using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface ITransactionRepo
    {
        public Task<Transaction> CreatTransaction(Transaction transaction);
        public Task<Transaction> GetTransaction(int transactionId);
        public Task<Transaction> UpdateTransaction(Transaction transaction);
        public Task<Transaction> DeleteTransaction(int transactionId);
        public Task<IList<Transaction>> GetAllTransaction();
        public Task<IList<Transaction>> GetAllActiveTransactionByTraderEmail(string traderEmail);
        public Task<Transaction> GetTransactionByReferenceNumber(string referenceNumber);
        public Task<IList<Transaction>> GetAllTransactionsByTraderEmail(string email);
        public Task<IList<Transaction>> GetAllTradersInTransaction(string referenceNumber);
        public Task<IList<Transaction>> GetAllTransactionByTransactionStatus(TransactionStatus status, string email);
        public Task<IList<Transaction>> GetInitiatedTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetProcessingTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetProcessingTransactionByTraderEmailSeller(string email);
        public Task<IList<Transaction>> GetAgreedTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetCompletedTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetRejectedTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetCancelledTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetAllTransactionByTransactionStatus(TransactionStatus status);
        public Task<IList<Transaction>> GetAllProcessingTransaction();
        public Task<IList<Transaction>> GetAllActiveTransaction();
        public Task<IList<Transaction>> GetAllInitiatedTransaction();
        public Task<IList<Transaction>> GetAllAgreedTransaction();
        public Task<Transaction> GetTransactionBySubtransactionReference(string subTransactionReference);
        public Task<IList<Transaction>> GetAllSubTransactionByProcessingTransaction();
        


    }
}