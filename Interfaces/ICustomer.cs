using project_backend.Models;

namespace project_backend.Interfaces
{
    public interface ICustomer
    {
        public Task<List<Customer>> GetAll();
        public Task<Customer> GetById(int id);
        public Task<Customer> GetFirstOrDefault();
        public Task<bool> CreateCustomer(Customer customer);

        public Task<Customer> findCustomerByDNI(string id);
    }
}
