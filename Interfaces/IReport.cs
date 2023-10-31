using proyecto_backend.Models;

namespace proyecto_backend.Interfaces
{
    public interface IReport
    {
        public Task<byte[]> ReportReceipt(Receipt receipt);
    }
}
