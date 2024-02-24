using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.Service.Stage;
using MatchingSystem.Service.Tests.Utils;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.UnitTests;

[TestFixture]
public class StageServiceTest
{
    private DiplomaMatchingContext _context;
    private IStageService _stageService;

    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.InMemory.json")]
    public void setUp()
    {
        //Юнит тест поэтому база InMemory - нас не интересют ошибки связанные с настройко справочников и таблиц только ошибки логики StageService
        _context = TestUtils.InitDb("appsettings.Test.InMemory.json");
        initTables();
        _context.SaveChanges();
        
        //Делаю Mock-класс для IMatchingRepository, в котором имитирую работу GetCurrentStage.
        //В юнит тесте тут эта имитация всегда работает ожидаемо и не изменится, если реальный метод поменяется - мы выявляем только ошибки StageService
        //В интеграционном тесте StageServiceIntegrationTest.cs будет
        //использоваться реальный метод чтобы выявлять ошибки связанные совместным использованием сервисов
        IMatchingRepository mockMatchingRepository = Substitute.For<IMatchingRepository>();
        //нюанс использования мока - если подать лямбду то она вычислится в момент вызова
        mockMatchingRepository.GetCurrentStage(1).Returns(х=>_context.Stages.SingleOrDefault(entity => entity.IsCurrent == true));
        //если подать выражение без лямбды -  она вычислится в момент вызова  setUp и ее значение будет закэшировано (те список в тот момент будет пуст)
        mockMatchingRepository.GetCurrentStage(2).Returns(_context.Stages.SingleOrDefault(entity => entity.IsCurrent == true));

        //Используем не мок а реальный класс, тк у него все равно нет параметров.
        //Его возможно мокнуть,но фактически пришлось просто переписать реализации его методов
        IStageRepository mockStageRepository = new StageRepository();
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _stageService = new StageService(mockStageRepository, mockMatchingRepository, loggerFactory.CreateLogger<StageService>());
    }

    private void initTables()
    {
        _context.Matchings.Add(new MatchingEntity() { MatchingId = 1, MatchingName = "TestMatching"});
    }

    [OneTimeTearDown]
    public void oneTimeTeardown()
    {
        TestUtils.DisposeDb();
    }
    
    [TearDown]
    public void teardown()//почистить контекст от отслеживаемых сущностей чтобы они не мешали в последующих тестах
    {
        var stagesToRemove = _context.Stages.ToList();
        _context.Stages.RemoveRange(stagesToRemove);
        _context.SaveChanges();
        TestUtils.DetachAllEntities(_context);
    }

    [TestCase(StageTypeEnum.MATCHING_INITIALIZATION, StageTypeEnum.MATCHING_PREPARATION)]
    [TestCase(StageTypeEnum.MATCHING_PREPARATION, StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES)]
    [TestCase(StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES, StageTypeEnum.ITERATION)]
    [TestCase(StageTypeEnum.ITERATION, StageTypeEnum.MANUAL_ADJUSTMENT)]
    [TestCase(StageTypeEnum.MANUAL_ADJUSTMENT, StageTypeEnum.FINAL)]
    [Test]
    public void testStageInitToTutors_ThenCurrentStageChanges(StageTypeEnum initialStageTypeId, StageTypeEnum finalStageTypeId)
    {
        _context.Stages.Add(new StageEntity()
        {
            StageId = 1,
            MatchingId = 1,
            IsCurrent = true,
            StageTypeId = (int)initialStageTypeId
        });
        _context.SaveChanges();
        
        _stageService.MoveToNextStage(1); //тестируемый метод

        var expectedStage = _context.Matchings.Single(entity => entity.MatchingId == 1)
            .Stages
            .Single(entity => entity.IsCurrent == true);
        var oldStage = _context.Stages.Single(stage => stage.StageId == 1);
        
        Assert.AreEqual(false, oldStage.IsCurrent);
        Assert.AreEqual((int)finalStageTypeId, expectedStage.StageTypeId);
    }
}