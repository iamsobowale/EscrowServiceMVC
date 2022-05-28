using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class AdminRepo:IAdminRepository
    {
        private readonly ApplicationContext _context;
        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin> DeleteAdminAsync(Admin admin)
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin> GetAdminAsync(int id)
        {
            return await _context.Admins.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IList<Admin>> GetAllAdminAsync()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            return await _context.Admins.SingleOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Admin> GetAdminByUserIdAsync(int userId)
        {
            return await _context.Admins.SingleOrDefaultAsync(c => c.UserId == userId);
        }
    }
}