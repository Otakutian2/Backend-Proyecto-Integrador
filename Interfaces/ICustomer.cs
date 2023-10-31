using proyecto_backend.Models;

namespace proyecto_backend.Interfaces
{
    public interface ICustomer
    {
        public Task<List<Customer>> GetAll();
        public Task<Customer> GetById(int id);
        public Task<Customer> GetFirstOrDefault();
        public Task<bool> CreateCustomer(Customer customer);
        public Task<Customer> FindCustomerByDni(string id);
        public Task<bool> IsDniUnique(string dni);
    }
}
