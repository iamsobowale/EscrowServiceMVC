using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface IAdminService
    {
        Task<BaseResponse> CreateAdminAsync(CreateAdminRequestModel requestModel);
        Task<BaseResponse> UpdateAdminAsync(UpdateAdminRequestModel requestModel, int id);
        Task<bool> DeleteAdminAsync(int id);
        Task<AdminResponseModel> GetAdminAsync(int id);
        Task<AdminResponsesModel> GetAllAdminsAsync();
        Task<AdminResponseModel> GetAdminByEmailAsync(string email);
        Task<AdminResponseModel> GetAdminByUserIdAsync(int userId);
    }
}