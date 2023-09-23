using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Interfaces
{
    public interface IReceiptType
    {
        public Task<List<ReceiptType>> GetAll();
        public Task<ReceiptType> GetById(int id);
        public Task<bool> CreateReceiptType(ReceiptType receiptType);
        public Task<bool> UpdateReceiptType(ReceiptType receiptType);
        public Task<bool> DeleteReceiptType(ReceiptType receiptType);
        public Task<int> Count(Expression<Func<ReceiptType, bool>> predicate = null);
    }
}
