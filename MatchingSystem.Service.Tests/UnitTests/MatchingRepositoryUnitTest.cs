using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.Service.Tests.Utils;
using Microsoft.EntityFrameworkCore;
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

    private DiplomaMatchingContext _context;

    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.InMemory.json")]
    public void setUp()
    {
        //Эмулирет базу данных в памяти, стартует полностью пустой и не выполняет проверки ограничений БД при вставке 
        _context = TestUtils.InitDb("appsettings.Test.InMemory.json");
        TestUtils.initDictionaryTables(_context);

        _context.Matchings.Add(matchingExpected);
        _context.SaveChanges();
        _context.Entry(matchingExpected).State = EntityState.Detached; 
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