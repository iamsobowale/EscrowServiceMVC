using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class PaymentRepo:IPaymentRepo
    {
        private readonly ApplicationContext _context;

        public PaymentRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> GetPayment(int paymentId)
        {
            return await _context.Payments.Include(c=>c.Transaction).Include(d => d.PaymentMethod).FirstOrDefaultAsync(x => x.PaymentId == paymentId);
        }

        public async Task<Payment> UpdatePayment(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> GetPaymentByReferenceNumber(string referenceNumber)
        {
            var getPayment = await _context.Payments.Include(c => c.Transaction).FirstOrDefaultAsync(c => c.ReferenceNumber == referenceNumber);
            return getPayment;
        }

        public async Task<IList<Payment>> GetPaymentByPaymentStatus(PaymentStatus paymentStatus)
        {
            var getPayment = await _context.Payments.Include(c => c.Transaction).Where(c => c.Status == paymentStatus).ToListAsync(
                );
            return getPayment;
        }

        public async Task<Payment> GetPaymentByPaymentMethod(string paymentMethod)
        {
            var getPayment = await _context.Payments.Include(c => c.Transaction).FirstOrDefaultAsync(c => c.PaymentMethod.Name == paymentMethod);
            return getPayment;
        }

        public async Task<IList<Payment>> GetAllSuccessfulPaymentByStatus(string transactionId)
        {
            var getPayment = await _context.Payments.Include(c => c.Transaction).Where(c => c.Status == PaymentStatus.Success && c.Transaction.ReferenceNumber == transactionId).ToListAsync();
            return getPayment;
        }

        public async Task<IList<Payment>> GetAllPendingPaymentByStatus(string transactionId)
        {
            var getPayment = await _context.Payments.Include(c => c.Transaction).Where(c => c.Status == PaymentStatus.Pending && c.Transaction.ReferenceNumber == transactionId).ToListAsync();
            return getPayment;
        }
    }
}