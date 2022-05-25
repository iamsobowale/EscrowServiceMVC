using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;

namespace EscrowService.Implementation.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDto> GetUser(int userId)
        {
            var getUser = await _userRepo.GetUser(userId);
            if (getUser == null)
            {
                return null;
            }

            return new UserDto
            {
                Email = getUser.Email,
                Password = getUser.Password,
                Role = getUser.Role,
            };

        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var getUser = await _userRepo.GetUserByEmail(email);
            if (getUser == null)
            {
                return null;
            }

            return new UserDto
            {
                Email = getUser.Email,
                Password = getUser.Password,
                Role = getUser.Role,
            };

        }
        

        public async Task<BaseResponse> Login(UserLoginRequest _request)
        {
            var getEmail = await _userRepo.GetUserByEmail(_request.Email);
            if (getEmail == null || getEmail.Password != _request.Password)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Invalid Email or Password",
                };
            }

            return new BaseResponse()
            {
                Message = "Login Successfully",
                IsSuccess = true,
            };

        }
    }
    
}