using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class UserRepo:IUserRepo
    {
        private readonly ApplicationContext _context;

        public UserRepo(ApplicationContext context)
        {
            _context = context;
        }

        public  async Task<User> CreateUser(User user)
        {
            var user1 = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(int userId)
        {
            var getUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return getUser;
        }

        public async Task<User> GetUserByEmail(string email)
        { 
            var  getByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return getByEmail;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
           return await _context.Traders.AnyAsync(c=>c.Email == email);
        }
    }
}