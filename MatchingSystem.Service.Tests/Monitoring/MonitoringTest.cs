using System;
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
    private IMonitoringService monitoringService;
    
    [SetUp]
    public void Setup()
    {
        var connString = "data source=localhost\\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True;";

      //  IStudentRepository studentRepository = Substitute.For<IStatisticsRepository>();
        IStudentRepository studentRepository = new StudentRepository(connString);
        IProjectRepository projectRepository = new ProjectRepository(connString);
        ITutorRepository tutorRepository = new TutorRepository(connString);
        IGroupRepository groupRepository = new GroupRepository(connString);


        monitoringService = new MonitoringService(studentRepository,tutorRepository,projectRepository,groupRepository);
    }
    
    [Test]
    public void testStatisicRealBD()
    {
        var result =  monitoringService.getMonitoringData(1);
        Console.WriteLine(result.ToString());
        Assert.AreEqual("", result.ToString());
        
    }
}