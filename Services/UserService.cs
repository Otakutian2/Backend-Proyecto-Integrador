using Microsoft.EntityFrameworkCore;
using project_backend.Data;
using project_backend.Interfaces;
using project_backend.Models;

namespace project_backend.Services
{
    public class UserService : IUser
    {
        private readonly CommandContext _context;

        public UserService(CommandContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.User
                .Include(u => u.Employee)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
