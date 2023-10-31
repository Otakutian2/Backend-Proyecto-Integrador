namespace proyecto_backend.Utils
{
    public static class GlobalUtils
    {
        public static int GenerateRandomNumber(int min, int max)
        {
            Random random = new();
            return random.Next(min, max);
        }
    }
}


