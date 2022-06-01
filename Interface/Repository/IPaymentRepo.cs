using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface IPaymentRepo
    {
        public Task<Payment> CreatePayment(Payment payment);
        public Task<Payment> GetPayment(string paymentId);
        public Task<Payment> UpdatePayment(Payment payment);
        public Task<Payment> DeletePayment(string paymentId);
        public Task<Payment> GetPaymentByTransactionId(string transactionId);
        public Task<Payment> GetPaymentByOrderId(string orderId);
        public Task<Payment> GetPaymentByPaymentStatus(string paymentStatus);
        public Task<Payment> GetPaymentByPaymentMethod(string paymentMethod);
    }
}