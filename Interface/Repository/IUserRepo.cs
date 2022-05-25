using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface IUserRepo
    {
        Task<User> CreateUser(User user);
        Task<User> GetUser(int userId);
        Task<User> GetUserByEmail(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}