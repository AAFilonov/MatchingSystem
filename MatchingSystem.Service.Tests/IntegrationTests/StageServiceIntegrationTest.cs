using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.Service.Stage;
using MatchingSystem.Service.Tests.Utils;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.IntegrationTests;

[TestFixture]
[NonParallelizable]
public class StageServiceIntegrationTest
{
    private DiplomaMatchingContext _context;
    private IStageService _stageService;
    private MatchingRepository _matchingRepository;
    private StageRepository _stageRepository;

    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.InMemory.json")]
    public void setUp()
    {
        /*
        //Тут должна была бы appsettings.Test.SqLiteMemory.json, но у нее есть баг связанный с
        //многоптоочным выполнением - одиночный тест срабатывает, а вот если запустить все вместе, то онав валится с ошибкой 
        Microsoft.EntityFrameworkCore.DbUpdateException : An error occurred while saving the entity changes. See the inner exception for details.
            ----> Microsoft.Data.Sqlite.SqliteException : SQLite Error 1: 'no such table: MatchingType'.
        Если для тестов такого плана лучше использовать SQLite
        */
        _context = TestUtils.InitDb("appsettings.Test.InMemory.json");
        _context.Database.EnsureCreated();
        TestUtils.initDictionaryTables(_context);
        _context.Matchings.Add(new MatchingEntity() { MatchingId = 1, MatchingName = "TestMatching", MatchingTypeId = 1});
        _context.SaveChanges();

        //Реальный класс
        _stageRepository = new StageRepository();
        //Тоже реальный класс - мы ожидаем что их взаимодействие с тестируемым тоже может создавать ошибки
        _matchingRepository = new MatchingRepository();

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _stageService = new StageService(_stageRepository, _matchingRepository,
            loggerFactory.CreateLogger<StageService>());
    }
    
    [OneTimeTearDown]
    public void teardown()
    {
        TestUtils.DisposeDb();
    }
    
    [Test]
    public void testAllStages_ThenCurrentStageChanges()//проверяем что стадии последовательно меняются в правильном порядке
    {
        _context.Stages.Add(new StageEntity()
        {
            StageId = 1,
            MatchingId = 1,
            IsCurrent = true,
            StageTypeId = (int) StageTypeEnum.MATCHING_INITIALIZATION
        });
        _context.SaveChanges();
        
        _stageService.MoveToNextStage(1);
        assertStageIsValid(StageTypeEnum.MATCHING_PREPARATION);
        
        _stageService.MoveToNextStage(1);
        assertStageIsValid(StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES);
        
        _stageService.MoveToNextStage(1);
        assertStageIsValid(StageTypeEnum.ITERATION);
        
        _stageService.MoveToNextStage(1);
        assertStageIsValid(StageTypeEnum.MANUAL_ADJUSTMENT);
        
        _stageService.MoveToNextStage(1);
        assertStageIsValid(StageTypeEnum.FINAL);
    }

    private void assertStageIsValid(StageTypeEnum stageTypeEnum)
    {
        var tutorStage = _matchingRepository.GetCurrentStage(1);
        Assert.AreEqual(true, tutorStage.IsCurrent);
        Assert.AreEqual((int)stageTypeEnum, tutorStage.StageTypeId);
        int countCurrent = _stageRepository.find(entity => entity.IsCurrent == true).ToList().Count;
        Assert.AreEqual(1, countCurrent);
    }
}