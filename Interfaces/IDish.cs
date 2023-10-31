using proyecto_backend.Dto;
using proyecto_backend.Models;
using System.Linq.Expressions;

namespace proyecto_backend.Interfaces
{
    public interface IDish
    {
        public Task<List<Dish>> GetAll();
        public Task<Dish> GetById(string id);
        public Task<bool> CreateDish(Dish Dish);
        public Task<bool> DeteleDish(Dish Dish);
        public Task<bool> UpdateDish(Dish Dish);
        public Task<List<DishOrderStatistics>> GetDishOrderStatistics();
        public Task<List<Dish>> GetDishByIdCategory(string id);
        public Task<bool> IsNameUnique(string name, string dishId = null);
        public Task<int> Count(Expression<Func<Dish, bool>> predicate = null);
    }
}
