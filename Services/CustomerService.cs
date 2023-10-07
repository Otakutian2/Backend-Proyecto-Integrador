using Microsoft.EntityFrameworkCore;
using project_backend.Data;
using project_backend.Interfaces;
using project_backend.Models;

namespace project_backend.Services
{
    public class CustomerService : ICustomer
    {
        private readonly CommandContext _context;

        public CustomerService(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCustomer(Customer customer)
        {
            bool result = false;

            try
            {
                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<Customer> GetById(int id)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == id);

            return customer;
        }

        public async Task<Customer> FindCustomerByDni(string id)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Dni == id);

            return customer;
        }

        public async Task<Customer> GetFirstOrDefault()
        {
            var customer = await _context.Customer.FirstOrDefaultAsync();

            return customer;
        }

        public async Task<List<Customer>> GetAll()
        {
            List<Customer> listCustomer = await _context.Customer.ToListAsync();

            return listCustomer;
        }
    }
}
