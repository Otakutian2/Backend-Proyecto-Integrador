using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Interfaces
{
    public interface ICash
    {
        public Task<List<Cash>> GetAll();
        public Task<Cash> GetById(int id);
        public Task<bool> CreateCash(Cash cash);
        public Task<bool> UpdateCash(Cash cash);
        public Task<bool> DeleteCash(Cash cash);
        public Task<int> Count(Expression<Func<Cash, bool>> predicate = null);
    }
}
