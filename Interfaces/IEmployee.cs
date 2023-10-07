using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Interfaces
{
    public interface IEmployee
    {
        public Task<List<Employee>> GetAll();
        public Task<Employee> GetById(int id);
        public Task<bool> CreateEmployee(Employee employee);
        public Task<bool> UpdateEmployee(Employee employee);
        public Task<bool> DeleteEmployee(Employee employee);
        public Task<bool> IsDniUnique(string dni, int? employeeId = null);
        public Task<bool> IsEmailUnique(string email, int? employeeId = null);
        public Task<bool> IsPhoneUnique(string phone, int? employeeId = null);
        public Task<int> Count(Expression<Func<Employee, bool>> predicate = null);
    }
}
