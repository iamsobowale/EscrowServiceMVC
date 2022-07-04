using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class TransactionTypeRepo:ITransactionTypeRepo
    {
        private readonly ApplicationContext _context;

        public TransactionTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TransactionType> GetTransactionTypeById(int id)
        {
            return await _context.TransactionTypes.FindAsync(id);
        }

        public async Task<TransactionType> GetTransactionTypeByReference(string reference, int transactionId)
        {
            return await _context.TransactionTypes.FirstOrDefaultAsync(x => x.Reference == reference && x.TransactionId == transactionId);
        }

        public async Task<TransactionType> GetTransactionTypeByRefrenceName(string reference)
        {

            return await _context.TransactionTypes.FirstOrDefaultAsync(x => x.Reference == reference);
        }

        public async Task<TransactionType> CreateTransactionType(TransactionType transactionType)
        {
            _context.TransactionTypes.AddRange(transactionType);
            await _context.SaveChangesAsync();
            return transactionType;
        }

        public async Task<IList<TransactionType>> CreateMultipleTransactionType(IList<TransactionType> transactionType)
        {
            _context.TransactionTypes.AddRange(transactionType);
            await _context.SaveChangesAsync();
            return transactionType;
        }

        public async Task<TransactionType> UpdateTransactionType(TransactionType transactionType)
        {
            
            _context.Entry(transactionType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return transactionType;
        }

        public async Task<bool> DeleteTransactionType(int id)
        {
            var transactionType = await _context.TransactionTypes.FindAsync(id);
            if (transactionType == null)
            {
                return false;
            }
            
            _context.TransactionTypes.Remove(transactionType);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public decimal GetSumAllTransactionTypeByTransactionReference(int transactionReference)
        {
            return _context.TransactionTypes.Where(x => x.TransactionId == transactionReference)
                .Sum(c => c.Price);
        }

        public async Task<IList<TransactionType>> GetAllTransactionTypeByReferenceNumber(int referenceId)
        {
            return await _context.TransactionTypes.Where(x => x.TransactionId == referenceId).ToListAsync();
        }
    }
}