using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;

namespace EscrowService.Interface.Service
{
    public interface IPaymentMethodService
    {
        Task<BaseResponse> CreatePaymentMethod(CreatePaymentMethodRequestModel _request);
        Task<BaseResponse> UpdatePaymentMethod(UpdatePaymentMethodRequestModel _request);
        Task<bool> DeletePaymentMethod(int id);
        Task<PaymentMethodResponseModel> GetPaymentMethod(int id);
        Task<List<PaymentMethodResponseModel>> GetAllPaymentMethod();
        Task<PaymentMethodResponseModel> GetPaymentMethodByName(string name);

    }
}