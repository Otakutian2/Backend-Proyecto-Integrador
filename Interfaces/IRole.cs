using project_backend.Models;

namespace project_backend.Interfaces
{
    public interface IRole
    {
        public Task<IEnumerable<Role>> GetAll();
        public Task<Role> GetById(int id);
        public Task<bool> CreateRole(Role role);
        public Task<bool> UpdateRole(Role role);
        public Task<bool> DeleteRole(Role role);

    }
}
