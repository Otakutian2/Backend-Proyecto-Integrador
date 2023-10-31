using proyecto_backend.Models;
using System.Linq.Expressions;

namespace proyecto_backend.Interfaces
{
    public interface IPaymentMethod
    {
        public Task<List<PaymentMethod>> GetAll();
        public Task<PaymentMethod> GetById(int id);
        public Task<bool> CreatePaymentMethod(PaymentMethod paymentMethod);
        public Task<bool> UpdatePaymentMethod(PaymentMethod paymentMethod);
        public Task<bool> DeletePaymentMethod(PaymentMethod paymentMethod);
        public Task<bool> IsPaymentMethodUnique(string paymentMethod, int? idPaymentMethod = null);
        public Task<int> Count(Expression<Func<PaymentMethod, bool>> predicate = null);
    }
}
