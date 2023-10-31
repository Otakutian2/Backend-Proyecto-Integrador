using Microsoft.EntityFrameworkCore;
using proyecto_backend.Data;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Utils;

namespace proyecto_backend.Services
{
    public class UserService : IUser
    {
        private readonly CommandContext _context;
        private readonly IEmail _emailService;

        public UserService(CommandContext context, IEmail emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.User
                .Include(u => u.Employee)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public bool VerifyCode(User user, int code)
        {
            return user.Code == code;
        }

        public async Task<bool> SendCode(User user)
        {
            bool result = false;

            try
            {
                var code = GlobalUtils.GenerateRandomNumber(1000, 9000);

                user.Code = code;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                Console.WriteLine($"Tu código de verificación es: {code}");

                //_emailService.SendEmail(email, "Código de verificación", $"Tu código de verificación es: {code}");
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> ChangePassword(User user, string password)
        {
            bool result = false;

            try
            {
                user.Password = SecurityUtils.HashPassword(password);
                user.Code = 0;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }
    }
}
