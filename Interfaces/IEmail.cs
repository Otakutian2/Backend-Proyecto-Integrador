namespace proyecto_backend.Interfaces
{
    public interface IEmail
    {
        public Task SendEmail(string recipient, string subject, string message);
    }
}
