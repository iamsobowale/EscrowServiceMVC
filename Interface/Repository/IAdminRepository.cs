using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface IAdminRepository
    {
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<Admin> DeleteAdminAsync(Admin admin);
        Task<Admin> GetAdminAsync(int id);
        Task<IList<Admin>> GetAllAdminAsync();
        Task<Admin> GetAdminByEmailAsync(string email);
        Task<Admin> GetAdminByUserIdAsync(int userId);
    }
}