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
    [DeploymentItem("appsettings.Test.SqliteMemory.json")]
    public void setUp()
    {
        _context = TestUtils.InitDb("appsettings.Test.SqliteMemory.json");
        _context.Database.EnsureCreated();
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
        _context.SaveChanges();
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