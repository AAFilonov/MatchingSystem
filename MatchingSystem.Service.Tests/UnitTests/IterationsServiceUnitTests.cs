using System;
using System.Linq;
using Common.Utils;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.StudentPreference;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using MatchingSystem.DataLayer.Feature.TutorChoice.repo;
using MatchingSystem.Service.Iterations;
using MatchingSystem.Service.Tests.Utils;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.UnitTests;

[TestFixture]
public class IterationsServiceTests
{
    private DiplomaMatchingContext _context;
    private IIterationsService _iterationsService;

    [SetUp]
    [DeploymentItem("appsettings.Test.InMemory.json")]
    public void setUp()
    {
        _context = TestUtils.InitDb("appsettings.Test.InMemory.json");
        TestUtils.initDictionaryTables(_context);
        initTables();

        IStudentRepository studentRepository = new StudentRepository();
        ITutorRepository tutorRepository = new TutorRepository();
        IProjectRepository projectRepository = new ProjectRepository();
        ITutorsChoiceRepository tutorsChoiceRepository= new TutorsChoiceRepository();
        _iterationsService = new IterationsService(tutorRepository, projectRepository, studentRepository, tutorsChoiceRepository);
    }

    [TearDown]
    public void oneTimeTeardown()
    {
        TestUtils.DisposeDb();
    }

    private void initTables()
    {
        _context.Stages.Add(new StageEntity{
                StageId = 1,
                MatchingId = 1,
                IsCurrent = true,
                StageTypeId = (int)StageTypeEnum.ITERATION,
                IterationNumber = 1
            }
        );
        _context.Matchings.Add(new MatchingEntity{ MatchingId = 1, MatchingName = "TestMatching" });

        _context.Students.Add(new StudentEntity{ StudentId = 1, MatchingId = 1, GroupId = 1 });
        _context.Students.Add(new StudentEntity{ StudentId = 2, MatchingId = 1, GroupId = 1 });
        _context.Students.Add(new StudentEntity{ StudentId = 3, MatchingId = 1, GroupId = 1 });

        _context.Tutors.Add(new TutorEntity{ TutorId = 1, MatchingId = 1, IsReadyToStart = true });
        _context.CommonQuotas.Add(new CommonQuotaEntity
            { CommonQuotaId = 1, TutorId = 1, StageId = 1, Qty = 2, QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE }
        );

        _context.Projects.Add(new ProjectEntity
            { ProjectId = 1, TutorId = 1, MatchingId = 1, ProjectQuotaQty = 1, ProjectName = "Tutor1 project 1" }
        );
        _context.Projects.Add(new ProjectEntity
            { ProjectId = 2, TutorId = 1, MatchingId = 1, ProjectQuotaQty = 1, ProjectName = "Tutor1 project 2" }
        );
        _context.Projects.Add(new ProjectEntity
            { ProjectId = 3, TutorId = 1, MatchingId = 1, ProjectQuotaQty = 1, ProjectName = "Tutor1 project 3" }
        );

        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 1, ProjectId = 1, OrderNumber = 1, IsAvailable = true, IsInUse = true, IsUsed = false }
        );
        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 1, ProjectId = 2, OrderNumber = 2, IsAvailable = true, IsInUse = false, IsUsed = false }
        );
        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 1, ProjectId = 3, OrderNumber = 3, IsAvailable = true, IsInUse = false, IsUsed = false }
        );

        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 2, ProjectId = 2, OrderNumber = 1, IsAvailable = true, IsInUse = true, IsUsed = false }
        );
        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 2, ProjectId = 3, OrderNumber = 2, IsAvailable = true, IsInUse = false, IsUsed = false }
        );
        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 2, ProjectId = 1, OrderNumber = 3, IsAvailable = true, IsInUse = false, IsUsed = false }
        );

        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 3, ProjectId = 3, OrderNumber = 1, IsAvailable = false, IsInUse = false, IsUsed = false }
        );
        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 3, ProjectId = 1, OrderNumber = 2, IsAvailable = false, IsInUse = false, IsUsed = false }
        );
        _context.StudentsPreferences.Add(new StudentsPreferenceEntity
            { StudentId = 3, ProjectId = 2, OrderNumber = 3, IsAvailable = false, IsInUse = false, IsUsed = false }
        );

        _context.SaveChanges();
    }

    [TestFixture]
    public class ValidationTests : IterationsServiceTests
    {
        [Test]
        public void testGetAllocation_ContentIsValid()
        {
            CreateTutorChoices();

            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);

            string json = JsonUtils.ToJson(allocation);
            Console.WriteLine(json);
            IOUtils.WriteStringToFile(json,
                "resources/json/IterationsService/testGetAllocation_actualOutput.json"
            );
            string jsonExpected =
                IOUtils.ReadFileAsString("resources/json/IterationsService/testGetAllocation_expected.json");
            Assert.AreEqual(jsonExpected, json);
        }

        [Test]
        public void testGetAllocation_linksAreValid()
        {
            CreateTutorChoices();

            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);

            // проверяем что ссылки в разных родительских объектах относятся к одним и тем же объектам в памяти
            var tutor = allocation.lecturers.Single(allocated => allocated.tutorId == 1);
            var project = allocation.projects.Single(allocated => allocated.projectId == 1);
            var student = allocation.students.Single(allocated => allocated.studentId == 1);

            Assert.AreSame(tutor.projects.First(), project);
            Assert.AreSame(student.preferences.First(), project);
            Assert.AreSame(tutor.preferences.Single(allocated => allocated.studentId == 1), student);
        }
    }

    [TestFixture]
    public class IsFinalTests : IterationsServiceTests
    {
        [Test]
        public void testIsFinal_WhenTutorQuotaIsNotFull_ThenFalse()
        {
            CreateTutorChoices();

            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            //меняем состояние базы до создания распределения
            var prefs = _context.StudentsPreferences.Where(entity => entity.StudentId == 3).ToList();
            foreach (StudentsPreferenceEntity studentsPreferenceEntity in prefs)
            {
                studentsPreferenceEntity.IsUsed = false; //все доступны
            }

            _context.SaveChanges();
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);
            var actual = _iterationsService.isInFinalIteration(allocation);
            Assert.AreEqual(false, actual);
        }


        [Test]
        public void testIsFinal_WhenTutorQuotaIsFull_ThenTrue()
        {
            CreateTutorChoices();

            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            //меняем состояние базы до создания распределения
            var prefs = _context.StudentsPreferences.Where(entity => entity.StudentId == 3).ToList();
            foreach (StudentsPreferenceEntity studentsPreferenceEntity in prefs)
            {
                studentsPreferenceEntity.IsUsed = true; //все использованные и неактивные
            }

            _context.SaveChanges();
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);

            var actual = _iterationsService.isInFinalIteration(allocation);
            Assert.AreEqual(true, actual);
        }
    }

    [TestFixture]
    public class IterationsTests : IterationsServiceTests
    {
        
        [Test]
        public void testStartIteration_whenFirstIteration()
        {
            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);

            //отвественный закончил прошлую итерацию, создался новый stage и началась новая - первично назначаем студентов преподам
            _iterationsService.startIteration(allocation);
            // _iterationsService.generateTutorChoices(allocation); - 
            
            string json = JsonUtils.ToJson(allocation);
            Console.WriteLine(json);
            IOUtils.WriteStringToFile(json, "resources/json/IterationsService/testStartIteration_actual.json");
            string jsonExpected =
                IOUtils.ReadFileAsString("resources/json/IterationsService/testStartIteration_expected.json");
            Assert.AreEqual(jsonExpected, json);
        }
        
        [Test]
        public void testGenerateTutorChoice()
        {
            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);

            //отвественный закончил прошлую итерацию, создался новый stage и началась новая - первично назначаем студентов преподам
            _iterationsService.startIteration(allocation);

            //генерируем предпочтения преподов, инзначально неопределенные (те в алфавитном порядке)
            //затем препод их руками меняет  и у станавливает такой порядок который он хочет
            _context.TutorsChoices.Add(new TutorsChoiceEntity{
                    ChoiceId = 1,
                    StudentId = 3,
                    ProjectId = 3,
                    StageId = 1,
                    IterationNumber = 1,
                    IsInQuota = false, //не назначен, в квоту не вошел
                    SortOrderNumber = Int16.MaxValue,
                    TypeId = (int)ChoiceTypeEnum.Auto,
                    IsFromPreviousIteration = false
                }
            );
            _context.TutorsChoices.Add(new TutorsChoiceEntity{
                    ChoiceId = 2,
                    StudentId = 1,
                    ProjectId = 1,
                    StageId = 1,
                    IterationNumber = 1,
                    IsInQuota = true, //назначен
                    SortOrderNumber = Int16.MaxValue,
                    TypeId = (int)ChoiceTypeEnum.Auto,
                    IsFromPreviousIteration = false
                }
            );

            _context.TutorsChoices.Add(new TutorsChoiceEntity{
                    ChoiceId = 3,
                    StudentId = 2,
                    ProjectId = 2,
                    StageId = 1,
                    IterationNumber = 1,
                    IsInQuota = true, //назначен
                    SortOrderNumber = Int16.MaxValue,
                    TypeId = (int)ChoiceTypeEnum.Auto,
                    IsFromPreviousIteration = false
                }
            );
            _context.SaveChanges();

            //затем отвественный заканчивает итерацию, студенты не вошедшие в квоты отсекаются
            _iterationsService.generateTutorChoices(allocation);

            //ожидаем что у преподов появляются предпочтения
            string json = JsonUtils.ToJson(allocation);
            Console.WriteLine(json);
            IOUtils.WriteStringToFile(json, "resources/json/IterationsService/testGenerateTutorChoice_actual.json");
            string jsonExpected =
                IOUtils.ReadFileAsString("resources/json/IterationsService/testGenerateTutorChoice_expected.json");
            Assert.AreEqual(jsonExpected, json);
        }

        [Test]
        public void testFinishiteration()
        {
            StageEntity currentStage =
                _context.Stages.Single(entity => entity.IsCurrent == true && entity.MatchingId == 1);
            DiplomaAllocation allocation = _iterationsService.getAllocation(currentStage);

            //отвественный закончил прошлую итерацию, создался новый stage и началась новая - первично назначаем студентов преподам
            _iterationsService.startIteration(allocation);

            //генерируем предпочтения преподов, инзначально неопределенные (те в алфавитном порядке)
            // _iterationsService.generateTutorChoices(allocation); - 
            //затем препод их руками меняет  и у станавливает такой порядок который он хочет
            _context.TutorsChoices.Add(new TutorsChoiceEntity{
                    ChoiceId = 1,
                    StudentId = 3,
                    ProjectId = 3,
                    StageId = 1,
                    IterationNumber = 1,
                    IsInQuota = false, //не назначен, в квоту не вошел
                    SortOrderNumber = Int16.MaxValue,
                    TypeId = (int)ChoiceTypeEnum.Auto,
                    IsFromPreviousIteration = false
                }
            );
            _context.TutorsChoices.Add(new TutorsChoiceEntity{
                    ChoiceId = 2,
                    StudentId = 1,
                    ProjectId = 1,
                    StageId = 1,
                    IterationNumber = 1,
                    IsInQuota = true, //назначен
                    SortOrderNumber = Int16.MaxValue,
                    TypeId = (int)ChoiceTypeEnum.Auto,
                    IsFromPreviousIteration = false
                }
            );

            _context.TutorsChoices.Add(new TutorsChoiceEntity{
                    ChoiceId = 3,
                    StudentId = 2,
                    ProjectId = 2,
                    StageId = 1,
                    IterationNumber = 1,
                    IsInQuota = true, //назначен
                    SortOrderNumber = Int16.MaxValue,
                    TypeId = (int)ChoiceTypeEnum.Auto,
                    IsFromPreviousIteration = false
                }
            );
            _context.SaveChanges();
            //инициализуируем предпочтения преподов в ДТО
            _iterationsService.generateTutorChoices(allocation);
            //затем отвественный заканчивает итерацию, студенты не вошедшие в квоты отсекаются
            _iterationsService.finishIteration(allocation);

            //ожидаем cтуденту 3 выровняет назначения по DB и в DTO
            string json = JsonUtils.ToJson(allocation);
            Console.WriteLine(json);
            IOUtils.WriteStringToFile(json, "resources/json/IterationsService/testFinishIteration_actual.json");
            string jsonExpected =
                IOUtils.ReadFileAsString("resources/json/IterationsService/testFinishIteration_expected.json");
            Assert.AreEqual(jsonExpected, json);
        }
    }


    private void CreateTutorChoices()
    {
        _context.TutorsChoices.Add(new TutorsChoiceEntity{
                ChoiceId = 1,
                StudentId = 3,
                ProjectId = 3,
                StageId = 1,
                IterationNumber = 1,
                IsInQuota = false, //не назначен, в квоту не вошел
                SortOrderNumber = Int16.MaxValue,
                TypeId = (int)ChoiceTypeEnum.Auto,
                IsFromPreviousIteration = false
            }
        );
        _context.TutorsChoices.Add(new TutorsChoiceEntity{
                ChoiceId = 2,
                StudentId = 1,
                ProjectId = 1,
                StageId = 1,
                IterationNumber = 1,
                IsInQuota = true, //назначен
                SortOrderNumber = Int16.MaxValue,
                TypeId = (int)ChoiceTypeEnum.Auto,
                IsFromPreviousIteration = false
            }
        );

        _context.TutorsChoices.Add(new TutorsChoiceEntity{
                ChoiceId = 3,
                StudentId = 2,
                ProjectId = 2,
                StageId = 1,
                IterationNumber = 1,
                IsInQuota = true, //назначен
                SortOrderNumber = Int16.MaxValue,
                TypeId = (int)ChoiceTypeEnum.Auto,
                IsFromPreviousIteration = false
            }
        );
        _context.SaveChanges();
    }
}