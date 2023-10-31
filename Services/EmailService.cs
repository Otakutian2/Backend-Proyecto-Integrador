using System.Net.Mail;
using System.Net;
using proyecto_backend.Interfaces;

namespace proyecto_backend.Services
{
    public class EmailService : IEmail
    {
        private readonly string FromEmail = "pruebacorreoCib@outlook.com";
        private readonly string Passsword = "CorreoGmailCom";

        public async Task SendEmail(string recipient, string subject, string message)
        {
            try
            {
                MailMessage mailMessage = new(FromEmail, recipient, subject, message);

                SmtpClient smtpClient = GetSmtpClient();

                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine("El correo se ha enviado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient smtpClient = new("smtp.office365.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(FromEmail, Passsword),
                EnableSsl = true
            };

            return smtpClient;
        }
    }
}
