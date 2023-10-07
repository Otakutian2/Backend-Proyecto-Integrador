using Microsoft.EntityFrameworkCore;
using project_backend.Data;
using project_backend.Interfaces;
using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Services
{
    public class PaymentMethodService : IPaymentMethod
    {
        private readonly CommandContext _context;

        public PaymentMethodService(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePaymentMethod(PaymentMethod paymentMethod)
        {
            bool result = false;

            try
            {
                _context.PaymentMethod.Add(paymentMethod);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            bool result = false;

            try
            {
                _context.Entry(paymentMethod).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            bool result = false;

            try
            {
                _context.PaymentMethod.Remove(paymentMethod);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<PaymentMethod> GetById(int id)
        {
            var paymentMethod = await _context.PaymentMethod.FirstOrDefaultAsync(x => x.Id == id);

            return paymentMethod;
        }

        public async Task<List<PaymentMethod>> GetAll()
        {
            return await _context.PaymentMethod.ToListAsync();
        }

        public async Task<bool> IsPaymentMethodUnique(string paymentMethod, int? idPaymentMethod = null)
        {
            if (idPaymentMethod != null)
            {
                return await _context.PaymentMethod.AllAsync(e => e.Name != paymentMethod || e.Id == idPaymentMethod);
            }

            return await _context.PaymentMethod.AllAsync(e => e.Name != paymentMethod);
        }

        public async Task<int> Count(Expression<Func<PaymentMethod, bool>> predicate = null)
        {
            return await (predicate != null ? _context.PaymentMethod.CountAsync(predicate) : _context.PaymentMethod.CountAsync());
        }
    }
}

