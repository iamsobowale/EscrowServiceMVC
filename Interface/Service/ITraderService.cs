using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Models;

namespace EscrowService.Interface.Service
{
    public interface ITraderService
    {
        Task<BaseResponse> CreateTraderAsync(CreateTraderRequestModel requestModel);
        Task<BaseResponse> UpdateTraderAsync(TraderUpdateRequestModel requestModel, int id);
        Task<bool> DeleteTraderAsync(int id);
        Task<TradersResponseModel> GetTraderAsync(int id);
        Task<TraderResponsesModel> GetAllTradersAsync();
        Task<TradersResponseModel> GetTraderByEmailAsync(string email);
        public Task<TraderResponsesModel> GetAllTradersInTransaction(string referenceNumber);
        Task<TradersResponseModel> GetTraderByUserIdAsync(int userId);
    }
}