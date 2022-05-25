using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int userId);
        Task<UserDto> GetUserByEmail(string email);
        Task<BaseResponse> Login(UserLoginRequest _request);
    }
}