using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class TransactionRepo:ITransactionRepo
    {
        private readonly ApplicationContext _context;

        public TransactionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreatTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> GetTransaction(int transactionId)
        {
           return await _context.Transactions.SingleOrDefaultAsync(s=>s.Id==transactionId);
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> DeleteTransaction(int transactionId)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(s => s.Id == transactionId);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IList<Transaction>> GetAllTransaction()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<IList<Transaction>> GetAllActiveTransactionByTraderEmail(string traderEmail)
        {
            return await _context.Transactions.Include(D => D.TradersTransactions).Where(e =>e.Status == TransactionStatus.isActive && e.SellerId == traderEmail).ToListAsync();

        }

        public async Task<Transaction> GetTransactionByReferenceNumber(string referenceNumber)
        {
            return await _context.Transactions.Include(C=> C.TransactionTypes).SingleOrDefaultAsync(s => s.ReferenceNumber == referenceNumber);
        }
        
        public async Task<IList<Transaction>> GetAllTradersInTransaction(string referenceNumber)
        {
            var getTransaction = await _context.Transactions.Include(c=>c.TradersTransactions).Where(c =>c.BuyerId==referenceNumber && c.SellerId==referenceNumber ).ToListAsync();
            return getTransaction;
        }

        public async Task<Transaction> GetTransactionByTransactionStatus(TransactionStatus status)
        {
            return await _context.Transactions.SingleOrDefaultAsync(s => s.Status == status);
        }


        public async Task<IList<Transaction>> GetProcessingTransactionByTraderEmail(string email)
        {
            return await _context.Transactions.Include(D => D.TradersTransactions).Where(e =>e.Status == TransactionStatus.isProcessing && e.BuyerId == email).ToListAsync();

        }

        public async Task<IList<Transaction>> GetAgreedTransactionByTraderEmail(string email)
        {
           return await _context.Transactions.Include(c => c.TradersTransactions).Where(d =>d.Status == TransactionStatus.isAgreed && d.BuyerId == email).ToListAsync();
            
        }

        public async Task<IList<Transaction>> GetAllTransactionsByTraderEmail(string email)
        {
           var getTransaction = await _context.Transactions.Include(c=>c.TradersTransactions).Where(c =>c.SellerId==email || c.BuyerId==email ).ToListAsync();
           return getTransaction;
        }


        public async Task<IList<Transaction>> GetAllTransactionByTransactionStatus(TransactionStatus status, string email)
        {
            return await _context.Transactions.Where(s => s.Status == status && s.BuyerId == email || s.SellerId == email).ToListAsync();
        }

        public async Task<IList<Transaction>> GetInitiatedTransactionByTraderEmail(string email)
        {
           var get = await _context.Transactions.Include(s => s.TradersTransactions).Where(d =>d.Status == TransactionStatus.isIntialized && d.SellerId == email ).ToListAsync();
              return get;
        }

        public async Task<IList<Transaction>> GetCompletedTransactionByTraderEmail(string email)
        { 
            return  await _context.Transactions.Include(c => c.TradersTransactions).Where(c => c.Status == TransactionStatus.IsCompleted && c.BuyerId == email || c.SellerId == email).ToListAsync();
        }

        public async Task<IList<Transaction>> GetRejectedTransactionByTraderEmail(string email)
        {
            return await _context.Transactions.Include(c => c.TradersTransactions).Where(c => c.Status == TransactionStatus.IsRejected && c.BuyerId == email || c.SellerId == email).ToListAsync();
        }

        public async Task<IList<Transaction>> GetCancelledTransactionByTraderEmail(string email)
        {
            return await _context.Transactions.Include(c => c.TradersTransactions).Where(c => c.Status == TransactionStatus.IsCancelled && c.BuyerId == email || c.SellerId == email).ToListAsync();
        }

        public async Task<IList<Transaction>> GetAllTransactionByTransactionStatus(TransactionStatus status)
        {
            return await _context.Transactions.Where(s => s.Status == status).ToListAsync();
        }

        public async Task<IList<Transaction>> GetAllProcessingTransaction()
        {
            return await _context.Transactions.Where(c => c.Status == TransactionStatus.isProcessing).ToListAsync();
        }

        public async Task<IList<Transaction>> GetAllActiveTransaction()
        {
            var getalltransac =await _context.Transactions.Where(c => c.Status == TransactionStatus.isActive).ToListAsync();
            return getalltransac;
        }

        public async Task<IList<Transaction>> GetAllInitiatedTransaction()
        {
            return await _context.Transactions.Where(c => c.Status == TransactionStatus.isIntialized).ToListAsync();
        }

        public async Task<IList<Transaction>> GetAllAgreedTransaction()
        {
            return await _context.Transactions.Where(c => c.Status == TransactionStatus.isAgreed).ToListAsync();
        }
    }
}