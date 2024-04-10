using MailKit.Net.Smtp;
using MatchingSystem.Constants;
using MimeKit;

namespace MatchingSystem.Service.Mail;

public class MailService : IMailService
{
    public void SendEmail(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(SystemParameters.getInstance().MAIL_FROM_NAME, SystemParameters.getInstance().MAIL_FROM_ADRESS));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        // Настройка SMTP клиента
        using var client = new SmtpClient();
        client.CheckCertificateRevocation = false;
        client.Connect(SystemParameters.getInstance().MAIL_HOST, SystemParameters.getInstance().MAIL_PORT, SystemParameters.getInstance().MAIL_USE_SSL);
        client.Authenticate(SystemParameters.getInstance().MAIL_USERNAME, SystemParameters.getInstance().MAIL_PASSWORD);
        client.Send(message);
        client.Disconnect(true);
    }
}