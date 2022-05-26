using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface IPaymentMethodRepo
    {
        Task<PaymentMethod> CreatePaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethod> UpdatePaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethod> DeletePaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethod> GetPaymentMethod(int id);
        Task<List<PaymentMethod>> GetAllPaymentMethod();
        Task<PaymentMethod> GetPaymentMethodByName(string name);
    }
}