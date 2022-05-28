using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface IAdminService
    {
        Task<BaseResponse> CreateAdminAsync(CreateTraderRequestModel requestModel);
        Task<BaseResponse> UpdateTraderAsync(TraderUpdateRequestModel requestModel, int id);
        Task<bool> DeleteTraderAsync(int id);
        Task<TradersResponseModel> GetTraderAsync(int id);
        Task<TraderResponsesModel> GetAllTradersAsync();
        Task<TradersResponseModel> GetTraderByEmailAsync(string email);
        Task<TradersResponseModel> GetTraderByUserIdAsync(int userId);
    }
}