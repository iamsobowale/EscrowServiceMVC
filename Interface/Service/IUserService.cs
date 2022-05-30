using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int userId);
        Task<UserDto> GetUserByEmail(string email);
        Task<UserDto> UpdateUser(UpdateUserRequestModel user, int id);
        Task<UserResponseModel> Login(UserLoginRequest _request);
    }
}