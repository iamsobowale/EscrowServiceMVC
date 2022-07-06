using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface ITransactionTypeRepo
    {
        Task<TransactionType> GetTransactionTypeById(int id);
        Task<TransactionType> GetTransactionTypeByReference(string reference, int transactionId);
        Task<TransactionType> GetTransactionTypeByRefrenceName(string reference);
        Task<TransactionType> CreateTransactionType(TransactionType transactionType);
        Task<IList<TransactionType>> CreateMultipleTransactionType(IList<TransactionType> transactionType);
        Task<TransactionType> UpdateTransactionType(TransactionType transactionType);
        Task<bool> DeleteTransactionType(int id);
        decimal GetSumAllTransactionTypeByTransactionReference(int transactionReference);
        Task<IList<TransactionType>> GetDeliveredSubTransaction(string transactionReference);
        Task<IList<TransactionType>> GetAcceptedSubTransaction();
        Task<IList<TransactionType>> GetAllTransactionTypeByReferenceNumber(int transactionId);

    }
}