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

        public Task<Payment> CreatePayment(Payment payment)
        {
            throw new System.NotImplementedException();
        }

        public Task<Payment> GetPayment(string paymentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Payment> UpdatePayment(Payment payment)
        {
            throw new System.NotImplementedException();
        }

        public Task<Payment> DeletePayment(string paymentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Payment> GetPaymentByTransactionId(string transactionId)
        { 
            var getPayment = await _context.Payments.Include(c=>c.Transaction).FirstOrDefaultAsync(c=>c.Transaction.ReferenceNumber==transactionId);
            return getPayment;
        }

        public Task<Payment> GetPaymentByOrderId(string orderId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Payment> GetPaymentByPaymentStatus(string paymentStatus)
        {
            throw new System.NotImplementedException();
        }

        public Task<Payment> GetPaymentByPaymentMethod(string paymentMethod)
        {
            throw new System.NotImplementedException();
        }
    }
}