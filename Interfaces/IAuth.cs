using project_backend.Models;

namespace project_backend.Interfaces
{
    public interface IAuth
    {
        public string GenerateJWTToken(User user);
        public Task<Employee> GetCurrentUser();
        public int GetCurrentUserId();
        public string GetCurrentUserRole();
    }
}
