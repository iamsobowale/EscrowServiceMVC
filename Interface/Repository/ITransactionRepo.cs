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
        public Task<Transaction> GetTransactionByReferenceNumber(string referenceNumber);
        public Task<IList<Transaction>> GetAllTransactionsByTraderEmail(string email);
        public Task<IList<Transaction>> GetAllTradersInTransaction(string referenceNumber);
        public Task<IList<Transaction>> GetAllTransactionByTransactionStatus(TransactionStatus status, string email);
        public Task<IList<Transaction>> GetInitiatedTransactionByTraderEmail(string email);
        public Task<IList<Transaction>> GetAgreedTransactionByTraderEmail(string email);
    }
}