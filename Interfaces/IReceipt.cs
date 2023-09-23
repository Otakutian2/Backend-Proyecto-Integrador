using project_backend.Dto;
using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Interfaces
{
    public interface IReceipt
    {
        public Task<List<Receipt>> GetAll();
        public Task<Receipt> GetById(int id);
        public Task<bool> CreateReceipt(Receipt receipt);
        public Task<List<SalesDataPerDate>> GetSalesDataPerDate();
        public Task<int> Count(Expression<Func<Receipt, bool>> predicate = null);
        public Task<int> ReceiptDetailsCount(Expression<Func<ReceiptDetails, bool>> predicate = null);
    }
}
