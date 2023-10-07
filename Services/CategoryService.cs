using project_backend.Interfaces;
using project_backend.Models;
using Microsoft.EntityFrameworkCore;
using project_backend.Data;
using System.Linq.Expressions;

namespace project_backend.Services
{
    public class CategoryService : ICategory
    {
        private readonly CommandContext _context;

        public CategoryService(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCategory(Category category)
        {
            bool result = false;

            try
            {
                var listCategory = await _context.Category.ToListAsync();

                category.Id = Category.GenerateId(listCategory);
                _context.Category.Add(category);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> DeteleCategory(Category category)
        {
            bool result = false;

            try
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<List<Category>> GetAll()
        {
            List<Category> listCategory = await _context.Category
                 .ToListAsync();

            return listCategory;
        }

        public async Task<Category> GetById(string id)
        {
            var category = await _context.Category
                 .FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            bool result = false;

            try
            {
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> IsNameUnique(string name, string categoryId = null)
        {
            if (categoryId != null)
            {
                return await _context.Category.AllAsync(e => e.Name != name || e.Id == categoryId);
            }

            return await _context.Category.AllAsync(e => e.Name != name);
        }

        public async Task<int> Count(Expression<Func<Category, bool>> predicate = null)
        {
            return await (predicate != null ? _context.Category.CountAsync(predicate) : _context.Category.CountAsync());
        }
    }
}
