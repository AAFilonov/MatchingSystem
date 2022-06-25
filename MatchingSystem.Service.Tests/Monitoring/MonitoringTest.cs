using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using MatchingSystem.Service.Monitoring;
using MatchingSystem.Service.Statistics;
using NSubstitute;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.Monitoring;

[TestFixture]
public class MonitoringTest
{
    private StatisticsService service;
    
    [SetUp]
    public void Setup()
    {
        IStatisticsRepository repository = Substitute.For<IStatisticsRepository>();
        var st = new StatisticsMain();
        st.StatName = "ололол!";
        repository.GetStatisticsGroups(4).Returns(new StatisticsMain[] { st});
         service = new StatisticsService(repository);
    }
    
    
    [Test]
    public void  testXXX_whenConditionXXX_ThenXXX()
    {
        
        MonitoringService service = new MonitoringService();

        var result =  service.getMonitoringData(4);
        
        Assert.AreEqual("Все хорошл ", result.ToString());
        
    }
    [Test]
    public void testStatisic()
    {
    
      

        var result =  service.GetStatisticsGroups(4);
    
        Assert.AreEqual("Все хорошл ", result.ToString());
        
    }
    
    [Test]
    public void testStatisicMock()
    {
        StatisticsRepository repository = new StatisticsRepository("data source=localhost\\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True;");
        StatisticsService service = new StatisticsService(repository);

        var result =  service.GetStatisticsGroups(4);
        
        Assert.AreEqual("Все хорошл ", result.ToString());
        
    }
}