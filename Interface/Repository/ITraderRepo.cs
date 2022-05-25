using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscrowService.Models;

namespace EscrowService.Interface.Repository
{
    public interface ITraderRepo
    {
        Task<Trader> CreateTraderAsync(Trader trader);
        Task<Trader> UpdateTraderAsync(Trader trader);
        Task<Trader> DeleteTraderAsync(Trader trader);
        Task<Trader> GetTraderAsync(int id);
        Task<IList<Trader>> GetAllTraderAsync(Expression<Func<Trader,bool>> expression);
        Task<Trader> GetTraderByEmailAsync(string email);
        Task<Trader> GetTraderByUserIdAsync(int userId);
        
    }
}