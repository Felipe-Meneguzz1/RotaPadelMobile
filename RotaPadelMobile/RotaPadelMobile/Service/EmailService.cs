using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace RotaPadelMobile.Services
{
    public class EmailService
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SenderEmail = "moneymapinvest1@gmail.com"; 
        private const string SenderPassword = "ayzo qoan oyve ioqd"; 
        private const string SenderName = "Rota Padel";

        public async Task<bool> EnviarCodigoRecuperacao(string emailDestino, string codigo)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(SenderName, SenderEmail));
                message.To.Add(new MailboxAddress("", emailDestino));
                message.Subject = "Recuperação de Senha - Rota Padel";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px;'>
                            <h2 style='color: #0636D4;'>Recuperação de Senha</h2>
                            <p>Você solicitou a recuperação de senha.</p>
                            <p>Seu código de verificação é:</p>
                            <h1 style='background-color: #C1EE0F; padding: 20px; text-align: center; 
                                       border-radius: 10px; letter-spacing: 5px;'>{codigo}</h1>
                            <p>Este código expira em 15 minutos.</p>
                            <p>Se você não solicitou esta recuperação, ignore este email.</p>
                            <br>
                            <p style='color: #666; font-size: 12px;'>
                                Equipe Rota Padel
                            </p>
                        </div>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(SenderEmail, SenderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao enviar email: {ex.Message}");
                return false;
            }
        }

        public string GerarCodigoRecuperacao()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Código de 6 dígitos
        }
    }
}