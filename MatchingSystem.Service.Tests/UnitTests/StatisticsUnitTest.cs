using System.Linq;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Statistics.repo;
using MatchingSystem.Service.Statistics;
using MatchingSystem.Service.Tests.Utils;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.UnitTests;

/*
Тесты предназанченные для сравнения результатов возврата функций статистики реализованных на SQL и C#
Для запуска необходимо закомментарить аннотацию, равернуть локульную MSSQL Server БД с навзанием DiplomaTest, накатить на нее 
бэкап базы, на котором есть хранимые процедуры - и сравнивать реузльтаты выборок по конркетному матчингу.
Это импользовалось для отладки шарповой реализации, в будущем может понадобится для отладки, или 
если будет реализовано тестирование на MSSQL базе данных при деплое в gitlab
 */
[Ignore("Тесты для ручного локального запуска")]
public class StatisticsUnitTest
{

    private StatisticsRepositoryOldDapper _statisticsRepositoryOldDapper;
    private IMainStatisticsService _mainStatisticsService;


    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.SqlServer.json")]
    public void setUp()
    {
        var _context = TestUtils.InitDb("appsettings.Test.SqlServer.json");
        _statisticsRepositoryOldDapper = new StatisticsRepositoryOldDapper(
            SystemParameters.getInstance().CONNECTION_STRING);
        IMatchingRepository matchingRepository = new MatchingRepository();
        _mainStatisticsService = new MainStatisticsService(matchingRepository);
    }

    [OneTimeTearDown]
    public void oneTimeTeardown()
    {
        //TestUtils.DisposeDb();
    }

    [Test]
    public void test_DapperStatisticsOnTutorsStage()
    {
        var expected = _statisticsRepositoryOldDapper.GetStatisticsMain(15, (int) StageTypeEnum.MATCHING_PREPARATION)
            .ToList();
        var actual = _mainStatisticsService.GetStatistics(15, (int) StageTypeEnum.MATCHING_PREPARATION).ToList();
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void test_DapperStatisticsOnStudentsStage()
    {
        var expected = _statisticsRepositoryOldDapper
            .GetStatisticsMain(15, (int) StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES).ToList();
        var actual = _mainStatisticsService
            .GetStatistics(15, (int) StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES).ToList();
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void test_DapperStatisticsOnIterationStage()
    {
        var expected = _statisticsRepositoryOldDapper
            .GetStatisticsMain(15, (int) StageTypeEnum.ITERATION).ToList();
        var actual = _mainStatisticsService
            .GetStatistics(15, (int) StageTypeEnum.ITERATION).ToList();
        Assert.AreEqual(expected, actual);
    }
}