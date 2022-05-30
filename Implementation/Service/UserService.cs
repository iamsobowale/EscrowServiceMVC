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

        public async Task<UserDto> UpdateUser(UpdateUserRequestModel user, int id)
        {
            var getUser = await _userRepo.GetUser(id);
            var updateUser = await _userRepo.UpdateUser(getUser);
            if (updateUser == null)
            {
                return null;
            }

            return new UserDto
            {
                Email = updateUser.Email,
                Password = updateUser.Password,
                Role = updateUser.Role,
            };

        }


        public async Task<UserResponseModel> Login(UserLoginRequest _request)
        {
            var getEmail = await _userRepo.GetUserByEmail(_request.Email);
            if (getEmail!=null && getEmail.Password == _request.Password)
            {
                return new UserResponseModel
                {
                    Data = new UserDto()
                    {
                        Email = getEmail.Email,
                    },
                    IsSuccess = true,
                    Message = "Login Successfully",
                };
            }
            return new UserResponseModel()
                {
                    IsSuccess = false,
                    Message = "Invalid Email or Password",
                    
                };

        }
    }
    
}