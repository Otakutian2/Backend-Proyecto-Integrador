using Microsoft.EntityFrameworkCore;
using proyecto_backend.Data;
using proyecto_backend.Dto;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using System.Linq.Expressions;

namespace proyecto_backend.Services
{
    public class DishService : IDish
    {
        private readonly CommandContext _context;

        public DishService(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateDish(Dish Dish)
        {
            bool result = false;

            try
            {
                var listDish = await _context.Dish.ToListAsync();

                Dish.Id = Dish.GenerateId(listDish);
                _context.Dish.Add(Dish);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> DeteleDish(Dish Dish)
        {
            bool result = false;

            try
            {
                _context.Dish.Remove(Dish);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<List<Dish>> GetAll()
        {
            List<Dish> listDish = await _context.Dish
                .Include(d => d.Category)
                .ToListAsync();

            return listDish;
        }

        public async Task<Dish> GetById(string id)
        {
            var dish = await _context.Dish
                    .Include(d => d.Category)
                   .FirstOrDefaultAsync(d => d.Id == id);

            return dish;
        }

        public async Task<bool> UpdateDish(Dish Dish)
        {
            bool result = false;

            try
            {
                _context.Entry(Dish).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<List<Dish>> GetDishByIdCategory(string id)
        {
            List<Dish> listDish = await _context.Dish
               .Include(d => d.Category)
               .Where(d => d.CategoryId == id)
               .ToListAsync();

            return listDish;
        }

        public async Task<int> Count(Expression<Func<Dish, bool>> predicate = null)
        {
            return await (predicate != null ? _context.Dish.CountAsync(predicate) : _context.Dish.CountAsync());
        }

        public async Task<List<DishOrderStatistics>> GetDishOrderStatistics()
        {
            var query = from dc in _context.CommandDetails
                        join c in _context.Command on dc.CommandId equals c.Id
                        join d in _context.Dish on dc.DishId equals d.Id
                        join ct in _context.Category on d.CategoryId equals ct.Id
                        where c.CommandStateId == 3
                        group dc by new { dc.DishId, d.Name, d.Image, CategoryName = ct.Name } into g
                        orderby g.Sum(dc => dc.OrderPrice) descending
                        select new DishOrderStatistics
                        {
                            DishId = g.Key.DishId,
                            Name = g.Key.Name,
                            ImgDish = g.Key.Image,
                            Category = g.Key.CategoryName,
                            TotalSales = g.Sum(dc => dc.OrderPrice),
                            QuantityOfDishesSold = g.Sum(dc => dc.DishQuantity)
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<bool> IsNameUnique(string name, string dishId = null)
        {
            if (dishId != null)
            {
                return await _context.Dish.AllAsync(e => e.Name != name || e.Id == dishId);
            }

            return await _context.Dish.AllAsync(e => e.Name != name);
        }
    }
}