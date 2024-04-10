using System;
using System.Linq;
using Common;
using Common.Utils;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.StudentPreference;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using MatchingSystem.DataLayer.Feature.TutorChoice.repo;
using MatchingSystem.Service.Iterations;
using MatchingSystem.Service.Stage;
using MatchingSystem.Service.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.feature.StageService;

[TestFixture]
[Ignore("Для ручного запуска")]//TODO - DPL-4 пока не сгенерю эталонные json-ы требует MS SQL базы
[NonParallelizable]
public class StageServiceRegressionTest //сопоставляем результаты работы реализации на SQL с реализацией на c#
{
    private DiplomaMatchingContext _context;
    private IStageService _stageService;
    private IterationsService _iterationsService;
    private MatchingRepositoryDapper _matchingRepositoryDapper;

    private const int MATHCING_ID = 1;
    
    [OneTimeSetUp]
    [DeploymentItem("appsettings.Test.SqliteMemory.json")]
    public void oneTimeSetUp()
    {
        _context = TestUtils.InitDb("appsettings.Test.SqliteMemory.json");
        
        var stageRepository = new StageRepository();
        var matchingRepository = new MatchingRepository();
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        IStudentRepository studentRepository = new StudentRepository();
        ITutorRepository tutorRepository = new TutorRepository();
        IProjectRepository projectRepository = new ProjectRepository();
        ITutorsChoiceRepository tutorsChoiceRepository= new TutorsChoiceRepository();
        _iterationsService = new IterationsService(tutorRepository, projectRepository, studentRepository, tutorsChoiceRepository);
        _stageService = new Stage.StageService(
            stageRepository, 
            matchingRepository,
            _iterationsService,
            loggerFactory.CreateLogger<Stage.StageService>());

        _matchingRepositoryDapper = new MatchingRepositoryDapper(SystemParameters.getInstance().CONNECTION_STRING);
        
        //инит без предпочтений
        string initSql = IOUtils.ReadFileAsString("resources/sql/StageServiceRegressionTest/init.sql");
        
        var affectedRows =_context.Database.ExecuteSqlRaw(initSql);
        Assert.NotZero(affectedRows);
        
        createPreferences();
    }

    private void createPreferences()
    {
        // все назанчаться сначала на первый проект, реджектуные из него на второй, редежкнутый из него на третий
        // итого будет p1:s1 p2:s2 p3:s3 и займет 2 итерации + итерация перехода на ручное распределение
        TestUtils.createUnusedPreference(_context,1,1,1);
        TestUtils.createUnusedPreference(_context,1,2,2);
        TestUtils.createUnusedPreference(_context,1,3,3);
        TestUtils.createUnusedPreference(_context,2,1,1);
        TestUtils.createUnusedPreference(_context,2,2,2);
        TestUtils.createUnusedPreference(_context,2,3,3);
        TestUtils.createUnusedPreference(_context,3, 1, 1);
        TestUtils.createUnusedPreference(_context,3, 2, 2);
        TestUtils.createUnusedPreference(_context,3, 3, 3);
        _context.SaveChanges();
    }


    [OneTimeTearDown]
    public void teardown()
    {
        TestUtils.DisposeDb();
    }

    [Ignore("Код для ручного запуска")]
    public void generateJson() //создаем json файлы используя вызов хранимки
    {
        //начальное состояние StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES
        SaveAllocationState("resources/json/StageService/regression/before_iterations_actual.json");
        
        var i = 0; //итерации включая ручное распределение
        while (true)
        {
            i++;
            _matchingRepositoryDapper.SetNextStage(MATHCING_ID);//cтало StageTypeEnum.ITERATION
            StageEntity currentStage = GetCurrentStage();
            if (currentStage.StageTypeId != (int)StageTypeEnum.ITERATION)
                break;
            SaveAllocationState($"resources/json/StageService/regression/iteration_{i}_expected.json");
           
        }
        SaveAllocationState($"resources/json/StageService/regression/manual_adjustment_actual.json");
    }
    

    [Test]
    public void test()// Сравнивает что результат адекватен
    {
        //TODO DPL-4  выполнять итерации 
    }
    
    private StageEntity SaveAllocationState(string outPath)
    {
        StageEntity currentStage = GetCurrentStage();
        Console.WriteLine($"Current stage {currentStage.StageId}, iteration {currentStage.IterationNumber}, stageType {currentStage.StageType.StageTypeName}");
        AllocationDamp damp = new AllocationDamp();
        damp.allocation = _iterationsService.getAllocation(currentStage);
        damp.stages = _context.Stages.Select(entity => StageDto.fromEntity(entity)).ToList();
        damp.choisesHistory = _context.TutorsChoices.Select(entity => new TutorChoiceDto(entity)).ToList();
        damp.finalChoises = _context.TutorsChoices
            .Where(entity =>entity.StageId.Equals(currentStage.StageId) )
            .Select(entity => new TutorChoiceDto(entity)).ToList();
        damp.preferences = _context.StudentsPreferences.Select(entity => new StudentPreferenceDto(entity)).ToList();
        
        string json = JsonUtils.ToJson(damp);
        IOUtils.WriteStringToFile(json, outPath);
        return currentStage;
    }

    private StageEntity GetCurrentStage()
    {
        StageEntity currentStage =
            _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == MATHCING_ID);
        return currentStage;
    }
}