using MatchingSystem.Service.Mail;
using MatchingSystem.Service.Tests.Utils;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.IntegrationTests;

[Ignore("Тест для ручной локальной првоерки отправки")]
[TestFixture]
public class MailServiceIntegrationTest
{
    private MailService _mailService;

    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.json")]
    public void setUp()
    {
        _mailService = new MailService();
       TestUtils.InitConfiguration("appsettings.Test.json");
    }

    [Test]
    public void testSendMail() //подразумевает что SMTP сервер запущен на localhost:25
    {
        _mailService.SendEmail("test@gmail.com", "Проверка", "Текст проверки");
    }
}