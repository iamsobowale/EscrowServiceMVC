using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;

namespace EscrowService.Interface.Service
{
    public interface IPaymentService
    {
        public Task<BaseResponse> CreatePayment(string transactionRefernce, string paymentMethod);
        public Task<PaymentResponseDto> GetPayment(int paymentId);
        public Task<PaymentResponseDto> GetPaymentByReferenceNumber(string referenceNumber);
        public Task<PaymentListResponseDto> GetPaymentByPaymentStatus(PaymentStatus paymentStatus);
        public Task<PaymentListResponseDto> GetSuccessfulPaymentByPaymentStatus(string transactionReference);
        public Task<PaymentListResponseDto> GetPendingPaymentByPaymentStatus(string transactionReference);
        
        public Task<PaymentListResponseDto> GetPaymentByPaymentMethod(string paymentMethod);
        public Task<string> MakePaymentWithPaystack(string clientid, string secretkey);
    }
}