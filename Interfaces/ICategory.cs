using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Interfaces
{
    public interface ICategory
    {
        public Task<List<Category>> GetAll();
        public Task<Category> GetById(string id);
        public Task<bool> CreateCategory(Category category);
        public Task<bool> DeteleCategory(Category category);
        public Task<bool> UpdateCategory(Category category);
        public Task<bool> IsNameUnique(string name, string categoryId = null);
        public Task<int> Count(Expression<Func<Category, bool>> predicate = null);
    }
}
