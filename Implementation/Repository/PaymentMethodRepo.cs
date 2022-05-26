using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.Context;
using EscrowService.Interface.Repository;
using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Implementation.Repository
{
    public class PaymentMrthodRepo:IPaymentMethodRepo
    {
        private readonly ApplicationContext _context;

        public PaymentMrthodRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PaymentMethod> CreatePaymentMethod(PaymentMethod paymentMethod)
        {
            await _context.PaymentMethods.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();
            return paymentMethod;
        }

        public async Task<PaymentMethod> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            _context.Update(paymentMethod);
            await _context.SaveChangesAsync();
            return paymentMethod;
        }

        public async Task<PaymentMethod> DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            _context.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            return paymentMethod;
        }

        public async Task<PaymentMethod> GetPaymentMethod(int id)
        {
             return await _context.PaymentMethods.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<PaymentMethod>> GetAllPaymentMethod()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod> GetPaymentMethodByName(string name)
        {
            return await _context.PaymentMethods.Where(c => c.Name == name).SingleOrDefaultAsync();
        }
    }
}