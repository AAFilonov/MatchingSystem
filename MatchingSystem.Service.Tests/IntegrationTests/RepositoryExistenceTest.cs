using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.Service.Tests.Utils;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.IntegrationTests;

[TestFixture]
public class RepositoryExistenceTest
{
    private MatchingRepository matchingRepository = new();
    private StageRepository stageRepository = new();

    private List<MatchingEntity> matchingExpected = new()
    {
        new()
        {
            MatchingId = 1,
            MatchingName = "Test matching",
            MatchingTypeId = 1,
        }
    };
    private List<StageEntity> stageExpected = new()
    {
        new()
        {
            MatchingId = 1,
            StageId = 1,
            StageTypeId= 1,
            IsCurrent = true
        }
    };

    
    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.SqliteMemory.json")]
    public void setUp()
    {
        //Запускает реальную базу данных и накатывает на нее миграции. В базе будет то что создано по итогу миграций
        var context = TestUtils.InitDb("appsettings.Test.SqliteMemory.json");
        context.StagesTypes.Add(new StagesType()
        {
            StageTypeId = 1,
            StageTypeCode = 1,
            StageTypeName = "Test",
            StageTypeNameRu = "Тест",
            
        });
        context.MatchingTypes.Add(new MatchingType()
        {
            MatchingTypeId = 1,
            MatchingTypeCode = "1",
            MatchingTypeName = "Test",
            MatchingTypeNameRu = "Тест",
            
        });
        context.SaveChanges();
        context.Matchings.AddRange(matchingExpected);
        context.SaveChanges();
        context.Stages.AddRange(stageExpected);
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
        var actual =  matchingRepository.findAll().ToList();
        Assert.AreEqual(matchingExpected, actual);
    }
        
    [Test]
    public void testStageTableExist()
    {
        var actual = stageRepository.findAll().ToList();
        Assert.AreEqual(stageExpected, actual);
    }
}