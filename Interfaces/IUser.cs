using project_backend.Models;

namespace project_backend.Interfaces
{
    public interface IUser
    {
        public Task<User> GetByEmail(string email);
    }
}
