using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.Service.Tests.Utils;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.UnitTests;

[TestFixture]
public class RepositoryExistenceUnitTest
{
    private MatchingRepository matchingRepository = new();

    private MatchingEntity matchingExpected = new()
    {
        MatchingId = 1,
        MatchingName = "Test matching",
        MatchingTypeId = 1,
    };
   
   
    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.InMemory.json")]
    public void setUp()
    {
        //Эмулирет базу данных в памяти, стартует полностью пустой и не выполняет проверки ограничений БД при вставке 
        var context = TestUtils.InitDb("appsettings.Test.InMemory.json");
        context.Matchings.Add(matchingExpected);
        context.SaveChanges();
    }
        
    [OneTimeTearDown]
    public void teardown()
    {
        TestUtils.DisposeDb();
    }
    
    [Test]
    public void testMatchingTableExist()
    {
        MatchingEntity actual =  matchingRepository.findById(1);
        Assert.AreEqual(matchingExpected, actual);
    }
        

}