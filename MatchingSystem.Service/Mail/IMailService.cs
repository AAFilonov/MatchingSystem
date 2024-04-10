namespace MatchingSystem.Service.Mail;

public interface IMailService
{
    public void SendEmail(string to, string subject, string body);
}