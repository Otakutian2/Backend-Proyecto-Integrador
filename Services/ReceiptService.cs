using Microsoft.EntityFrameworkCore;
using proyecto_backend.Data;
using proyecto_backend.Dto;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using System.Linq.Expressions;

namespace proyecto_backend.Services
{
    public class ReceiptService : IReceipt
    {
        private readonly CommandContext _context;

        public ReceiptService(CommandContext context)
        {
            _context = context;
        }

        public async Task<List<Receipt>> GetAll()
        {
            return await _context.Receipt
                .Include(x => x.Employee.User)
                .Include(x => x.Employee.Role)
                .Include(x => x.Cash)
                .ThenInclude(x => x.Establishment)
                .Include(x => x.Customer)
                .Include(x => x.ReceiptType)
                .Include(x => x.ReceiptDetailsCollection)
                .ThenInclude(x => x.PaymentMethod)
                .Include(x => x.Command)
                .Include(x => x.Command.TableRestaurant)
                .Include(x => x.Command.Employee)
                .Include(x => x.Command.Employee.User)
                .Include(x => x.Command.Employee.Role)
                .Include(x => x.Command.CommandState)
                .Include(x => x.Command.CommandDetailsCollection)
                .ThenInclude(x => x.Dish)
                .Include(x => x.Command.CommandDetailsCollection)
                .ThenInclude(x => x.Dish.Category)
                .ToListAsync();
        }

        public async Task<Receipt> GetById(int id)
        {
            return await _context.Receipt
                .Include(x => x.Employee.User)
                .Include(x => x.Employee.Role)
                .Include(x => x.Cash)
                .ThenInclude(x => x.Establishment)
                .Include(x => x.Customer)
                .Include(x => x.ReceiptType)
                .Include(x => x.ReceiptDetailsCollection)
                .ThenInclude(x => x.PaymentMethod)
                .Include(x => x.Command)
                .Include(x => x.Command.TableRestaurant)
                .Include(x => x.Command.Employee)
                .Include(x => x.Command.Employee.User)
                .Include(x => x.Command.Employee.Role)
                .Include(x => x.Command.CommandState)
                .Include(x => x.Command.CommandDetailsCollection)
                .ThenInclude(x => x.Dish)
                .Include(x => x.Command.CommandDetailsCollection)
                .ThenInclude(x => x.Dish.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> CreateReceipt(Receipt receipt)
        {
            using var transaction = _context.Database.BeginTransaction();
            bool result = false;

            try
            {
                _context.Receipt.Add(receipt);
                await _context.SaveChangesAsync();

                transaction.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
            }

            return result;
        }

        public async Task<int> Count(Expression<Func<Receipt, bool>> predicate = null)
        {
            return await (predicate != null ? _context.Receipt.CountAsync(predicate) : _context.Receipt.CountAsync());
        }

        public async Task<int> ReceiptDetailsCount(Expression<Func<ReceiptDetails, bool>> predicate = null)
        {
            return await (predicate != null ? _context.ReceiptDetails.CountAsync(predicate) : _context.ReceiptDetails.CountAsync());
        }

        public async Task<List<SalesDataPerDate>> GetSalesDataPerDate()
        {
            var query = from v in _context.Receipt
                        join c in _context.Command on v.CommandId equals c.Id
                        join dc in _context.CommandDetails on c.Id equals dc.CommandId
                        join d in _context.Dish on dc.DishId equals d.Id
                        group new { v, dc } by v.CreatedAt.Date into g
                        orderby g.Sum(x => x.v.TotalPrice) descending
                        select new SalesDataPerDate
                        {
                            CreatedAt = g.Key,
                            AccumulatedSales = g.Sum(x => x.v.TotalPrice),
                            NumberOfGeneratedReceipts = g.Select(x => x.v.Id).Distinct().Count(),
                            QuantityOfDishSales = g.Sum(x => x.dc.DishQuantity),
                            BestSellingDish = (from v2 in _context.Receipt
                                               join c2 in _context.Command on v2.CommandId equals c2.Id
                                               join dc2 in _context.CommandDetails on c2.Id equals dc2.CommandId
                                               join d2 in _context.Dish on dc2.DishId equals d2.Id
                                               where v2.CreatedAt.Date == g.Key
                                               group dc2 by new { d2.Name } into g2
                                               orderby g2.Sum(dc2 => dc2.DishQuantity) descending
                                               select g2.Key.Name).FirstOrDefault()
                        };

            var result = await query.ToListAsync();
            return result;
        }
    }
}
