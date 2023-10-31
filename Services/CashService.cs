
using Microsoft.EntityFrameworkCore;
using proyecto_backend.Data;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using System.Linq.Expressions;

namespace proyecto_backend.Services
{
    public class CashService : ICash
    {
        private readonly CommandContext _context;

        public CashService(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCash(Cash cash)
        {
            bool result = false;

            try
            {
                _context.Cash.Add(cash);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> DeleteCash(Cash cash)
        {
            bool result = false;

            try
            {
                _context.Cash.Remove(cash);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<List<Cash>> GetAll()
        {
            List<Cash> listCash = await _context.Cash.Include(c => c.Establishment).ToListAsync();

            return listCash;
        }

        public async Task<Cash> GetById(int id)
        {
            var cash = await _context.Cash.Include(c => c.Establishment)
                .FirstOrDefaultAsync(x => x.Id == id);

            return cash;
        }

        public async Task<bool> UpdateCash(Cash cash)
        {
            bool result = false;

            try
            {
                _context.Entry(cash).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<int> Count(Expression<Func<Cash, bool>> predicate = null)
        {
            return await (predicate != null ? _context.Cash.CountAsync(predicate) : _context.Cash.CountAsync());
        }
    }
}
