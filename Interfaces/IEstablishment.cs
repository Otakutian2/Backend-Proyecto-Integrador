using project_backend.Models;

namespace project_backend.Interfaces
{
    public interface IEstablishment
    {
        public Task<List<Establishment>> GetAll();
        public Task<Establishment> GetById(int id);
        public Task<Establishment> GetFirstOrDefault();
        public Task<bool> CreateEstablishment(Establishment establishment);
        public Task<bool> UpdateEstablishment(Establishment establishment);
        public Task<bool> DeleteEstablishment(Establishment establishment);
    }
}
