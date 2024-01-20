using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using MatchingSystem.Service.DocumentsProcessing;
using MatchingSystem.Service.Exception;
using MatchingSystem.Service.MatchingInitialization;
using MatchingSystem.Service.Student;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace MatchingSystem.UI.ApiControllers;

[ApiController]
public class MatchingInitializationController : ControllerBase
{
    private readonly IDocumentsProcessingService documentsProcessingService;
    private readonly ILogger<MatchingInitializationController> logger;
    private readonly IMatchingInitializationService matchingInitializationService;
    private readonly IStudentsParsingService studentsParsingService;
    private readonly ITutorRepository tutorRepository;
    private readonly IStudentService studentService;
    private readonly IStudentRepository studentRepository;

    private readonly ITutorsParsingService tutorsParsingService;

    public MatchingInitializationController(IDocumentsProcessingService documentsProcessingService,
        ILogger<MatchingInitializationController> logger, IStudentsParsingService studentsParsingService,
        ITutorRepository tutorRepository, ITutorsParsingService tutorsParsingService, IStudentService studentService,
        IStudentRepository studentRepository, IMatchingInitializationService matchingInitializationService)
    {
        this.documentsProcessingService = documentsProcessingService;

        this.logger = logger;
        this.studentsParsingService = studentsParsingService;
        this.tutorRepository = tutorRepository;

        this.tutorsParsingService = tutorsParsingService;
        this.studentService = studentService;
        this.studentRepository = studentRepository;
        this.matchingInitializationService = matchingInitializationService;
    }


    // шаг 1
    [Route("api/[controller]/upload_students_data")]
    [HttpPost]
    public async Task<IActionResult> uploadStudentsDataAsync(IFormCollection files)
    {
        var data = files.Files[0];

        logger.LogInformation("INFO: Accepting student data upload request. Filename is {}", data.FileName);

        await using var memoryStream = new MemoryStream();
        await data.CopyToAsync(memoryStream);
        var sessionData = HttpContext.Session.Get<SessionData>("Data");

        var package = new ExcelPackage(memoryStream);
        var groups = studentsParsingService.parseStudentGroupData(package);
        var students = studentsParsingService.tryToParseStudentData(package);

        sessionData.matchingInitData.studentRecords = students;
        sessionData.matchingInitData.groupRecords = groups;

        HttpContext.Session.Set("Data", sessionData);
        logger.LogInformation("INFO: Student data upload for file {} was successful. Session context was updated",
            data.FileName);
        return new JsonResult(students);
    }

    // шаг 2
    [Route("api/[controller]/upload_tutors_data")]
    [HttpPost]
    public async Task<IActionResult> uploadTutorDataAsync(IFormCollection files)
    {
        var data = files.Files[0];
        string ArrayToString(List<string> list) => string.Join(", ", list.ToArray());
        
        logger.LogInformation("INFO: Accepting tutors data upload request. Filename is {}", data.FileName);

        await using var memoryStream = new MemoryStream();
        await data.CopyToAsync(memoryStream);
        var sessionData = HttpContext.Session.Get<SessionData>("Data");

        var package = new ExcelPackage(memoryStream);
        var tutorGroups = tutorsParsingService.parseTutorGroupData(package);
        var tutorGroupsNames = tutorGroups.Select(dto => dto.name).ToList();

        var tutorGroupsMatchWithStudentGroups = true;
        var studentGroupNames = sessionData.matchingInitData.groupRecords.Select(initDto => initDto.name).ToList();
        tutorGroupsMatchWithStudentGroups = tutorGroupsMatchWithStudentGroups && studentGroupNames.Count.Equals(tutorGroups.Count);
        tutorGroups.ForEach(dto =>
        {
            tutorGroupsMatchWithStudentGroups =
                tutorGroupsMatchWithStudentGroups && studentGroupNames.Any(name => name.Equals(dto.name));

            if (!tutorGroupsMatchWithStudentGroups)
                throw new InputDataException("группы преподавателей [" + ArrayToString(tutorGroupsNames) +
                                             "] не соотвествуют группам заданным для студентов [" +ArrayToString(studentGroupNames)+"]");
        });

        var tutors = tutorsParsingService.tryToParseTutorsData(package);


        var dublicatedTutors = tutors.GroupBy(x => x.nameAbbreviation)
            .Where(g => g.Count() > 1)
            .ToDictionary(x => x.Key, y => y.Count())
            .Where(pair => pair.Value > 0).ToList();
        if (dublicatedTutors.Count > 0) throw new InputDataException("в списке преподавателей присутсвуют дубликаты: "
                                                                     +ArrayToString(dublicatedTutors.Select(pair =>pair.Key).ToList()));

        var existing_tutors = tutorRepository.GetTutorsViews();

        tutors.ToList().ForEach(tutorDto =>
        {
            if (!existing_tutors.Any(tutor => tutor.NameAbbreviation.Equals(tutorDto.nameAbbreviation)))
                throw new InputDataException("преподватель с ФИО " + tutorDto.nameAbbreviation + " не зарегестрирован");
        });

        sessionData.matchingInitData.tutorRecords = tutors;

        HttpContext.Session.Set("Data", sessionData);
        logger.LogInformation("INFO: Tutors data upload for file {} was successful. Session context was updated",
            data.FileName);
        return new JsonResult(tutors);
    }

    // шаг 3
    [Route("api/[controller]/create_matching")]
    [HttpPost]
    public IActionResult createNewMatching(MatchingInitData data)
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        sessionData.matchingInitData.matching = data.matching;
        HttpContext.Session.Set("Data", sessionData);
        var currentMatchingInitData = sessionData.matchingInitData;
        currentMatchingInitData.matching = data.matching;
        data.creatorId = sessionData.UserDto.UserId;
        //var updatedData = matchingInitializationService.createMatching(currentMatchingInitData);
        data.groupRecords = data.tutorRecords.FirstOrDefault().groups;
        
        var updatedData = matchingInitializationService.InitNewMatching(data);
        sessionData.matchingInitData = updatedData;
        HttpContext.Session.Set("Data", sessionData);

        return Ok();
    }

    //шаг 4 
    [Route("api/[controller]/student_data_report")]
    [HttpGet]
    public FileResult getStudentDataReport()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var currentMatchingId = sessionData.SelectedMatching;

        var students = studentRepository.getStundentsView().Where(student => student.MatchingId == currentMatchingId)
            .ToList();
        var studentDos = studentService.resetPasswords(students);

        var package = documentsProcessingService.formStudentDataReport(studentDos);
        return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Reports.xlsx");
    }

    [Route("api/[controller]/matching_data_sync")]
    [HttpGet]
    public IActionResult getMatchingData()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var data = sessionData.matchingInitData;
        return new JsonResult(data);
    }
}