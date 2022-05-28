using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface IAdminService
    {
        Task<BaseResponse> CreateAdminAsync(CreateAdminRequestModel requestModel);
        Task<BaseResponse> UpdateTraderAsync(UpdateAdminRequestModel requestModel, int id);
        Task<bool> DeleteAdminAsync(int id);
        Task<AdminResponseModel> GetAdminAsync(int id);
        Task<AdminResponsesModel> GetAllTradersAsync();
        Task<AdminResponseModel> GetTraderByEmailAsync(string email);
        Task<AdminResponseModel> GetTraderByUserIdAsync(int userId);
    }
}