using proyecto_backend.Models;

namespace proyecto_backend.Interfaces
{
    public interface IUser
    {
        public Task<User> GetByEmail(string email);
        public bool VerifyCode(User user, int code);
        public Task<bool> SendCode(User user);
        public Task<bool> ChangePassword(User user, string password);
    }
}
