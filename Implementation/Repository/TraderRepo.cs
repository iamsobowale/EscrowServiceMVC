using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class TraderRepo :ITraderRepo
    {
        private readonly ApplicationContext _context;

        public TraderRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Trader> CreateTraderAsync(Trader trader)
        {
            await _context.Traders.AddAsync(trader);
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> UpdateTraderAsync(Trader trader)
        {
            _context.Traders.Update(trader);
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> DeleteTraderAsync(Trader trader)
        {
            _context.Traders.Remove(trader);
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> GetTraderAsync(int id)
        {
            return await _context.Traders.Include(c=>c.User).FirstOrDefaultAsync(i=>i.Id==id && i.IsDeleted==false);
        }

        public async Task<IList<Trader>> GetAllTraderAsync(Expression<Func<Trader, bool>> expression)
        {
            return await _context.Traders.Where(c => c.IsDeleted == false).ToListAsync();
        }
        
        public async Task<Trader> GetTraderByEmailAsync(string email)
        {
          var getEmail = await _context.Traders.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
          return getEmail;
        }

        public async Task<Trader> GetTraderByUserIdAsync(int userId)
        {
           return await _context.Traders.FirstOrDefaultAsync(a=>a.UserId == userId && a.IsDeleted == false);
            
        }
        
    }
}