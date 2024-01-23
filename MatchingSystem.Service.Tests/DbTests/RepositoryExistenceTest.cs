using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.Service.Tests.Utils;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.DbTests;

[TestFixture]
public class RepositoryExistenceTest
{
    private MatchingRepository matchingRepository = new();
    private StageRepository stageRepository = new();

    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.json")]
    public void setUp()
    {
        TestUtils.InitDb();
    }
        
    [OneTimeTearDown]
    public void teardown()
    {
        TestUtils.DisposeDb();
    }
    
    [Test]
    public void testMatchingTableExist()
    {
        var t =  matchingRepository.GetMatchings();;
        var expected = new List<MatchingEntity>();
        Assert.AreEqual(t, expected);
    }
        
    [Test]
    public void testStageTableExist()
    {
        var t = stageRepository.findAll() .ToList();
        var expected = new List<StageEntity>();
        Assert.AreEqual(t, expected);
    }
}