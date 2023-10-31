using Microsoft.EntityFrameworkCore;
using proyecto_backend.Data;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using System.Linq.Expressions;

namespace proyecto_backend.Services
{
    public class ReceiptTypeServices : IReceiptType
    {
        private readonly CommandContext _context;

        public ReceiptTypeServices(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReceiptType(ReceiptType receiptType)
        {
            bool result = false;

            try
            {
                _context.ReceiptType.Add(receiptType);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> DeleteReceiptType(ReceiptType receiptType)
        {
            bool result = false;

            try
            {
                _context.ReceiptType.Remove(receiptType);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<List<ReceiptType>> GetAll()
        {
            return await _context.ReceiptType
                .Include(x => x.ReceiptCollection)
                .ToListAsync();
        }

        public Task<ReceiptType> GetById(int id)
        {
            return _context.ReceiptType.Include(x => x.ReceiptCollection).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateReceiptType(ReceiptType receiptType)
        {
            bool result = false;

            try
            {
                _context.Entry(receiptType).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<int> Count(Expression<Func<ReceiptType, bool>> predicate = null)
        {
            return await (predicate != null ? _context.ReceiptType.CountAsync(predicate) : _context.ReceiptType.CountAsync());
        }

    }
}
