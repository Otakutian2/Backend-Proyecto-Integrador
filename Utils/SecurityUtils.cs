namespace project_backend.Utils
{
    public static class SecurityUtils
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool CheckPassword(string hashPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
