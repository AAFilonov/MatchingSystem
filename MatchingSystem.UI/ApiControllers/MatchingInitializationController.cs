using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.Service.DocumentsProcessing;
using MatchingSystem.Service.Exception;
using MatchingSystem.Service.MatchingInitialization;
using MatchingSystem.Service.Tutor;
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
    private readonly ITutorService tutorService;

    private readonly ITutorsParsingService tutorsParsingService;
    private readonly IStudentsParsingService studentsParsingService;
    private readonly IMatchingInitializationService matchingInitializationService;
    private readonly ILogger<MatchingInitializationController> logger;
    private readonly IDocumentsProcessingService documentsProcessingService;
    private readonly ITutorRepository tutorRepository;
    private MatchingInitData buffer; //TODO  костыль для демо

    public MatchingInitializationController(ITutorService tutorService,
        ILogger<MatchingInitializationController> logger, IMatchingInitializationService matchingInitializationService,
        ITutorsParsingService tutorsParsingService, IStudentsParsingService studentsParsingService,
        IDocumentsProcessingService documentsProcessingService, ITutorRepository tutorRepository)
    {
        this.tutorService = tutorService;
        this.logger = logger;
        this.matchingInitializationService = matchingInitializationService;
        this.tutorsParsingService = tutorsParsingService;
        this.studentsParsingService = studentsParsingService;
        this.documentsProcessingService = documentsProcessingService;
        this.tutorRepository = tutorRepository;
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

        ExcelPackage package = new ExcelPackage(memoryStream);
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

        logger.LogInformation("INFO: Accepting tutors data upload request. Filename is {}", data.FileName);

        await using var memoryStream = new MemoryStream();
        await data.CopyToAsync(memoryStream);
        var sessionData = HttpContext.Session.Get<SessionData>("Data");

        ExcelPackage package = new ExcelPackage(memoryStream);
        var groups = tutorsParsingService.parseTutorGroupData(package);


        bool tutorGroupsMatchWithStudentGroups = true;
        var groupNames = sessionData.matchingInitData.groupRecords.Select(initDto => initDto.name).ToList();
        tutorGroupsMatchWithStudentGroups = tutorGroupsMatchWithStudentGroups && groupNames.Count.Equals(groups.Count);
        groups.ForEach(dto =>
        {
            tutorGroupsMatchWithStudentGroups = tutorGroupsMatchWithStudentGroups && groupNames.Any(name => name.Equals(dto.name));
        } );
       
    
        if (!tutorGroupsMatchWithStudentGroups)
        {
            throw new InputDataException("группы преподавателей не соотвествуют группам заданным для студентов");
        }

        var tutors = tutorsParsingService.tryToParseTutorsData(package);
        
      
        var dublicatedTutors = tutors.GroupBy(x => x.nameAbbreviation)
            .Where(g => g.Count() > 1)
            .ToDictionary(x => x.Key, y => y.Count())
            .Where(pair =>pair.Value>0 ).ToList();
        if (dublicatedTutors.Count > 0)
        {
            throw new InputDataException("в списке преподавателей присутсвуют дубликаты");
        }
        
        var existing_tutors = tutorRepository.GetAllTutors().ToList();

        tutors.ToList().ForEach(tutorDto =>
        {
            if (!existing_tutors.Any(tutor => tutor.NameAbbreviation.Equals(tutorDto.nameAbbreviation)))
            {
                throw new InputDataException("преподватель с ФИО " + tutorDto.nameAbbreviation + " не зарегестрирован");
            }
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
    public IActionResult createNewMatching([FromBody] MatchingInitData data)
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        sessionData.matchingInitData.matching = data.matching;
        HttpContext.Session.Set("Data", sessionData);
        var currentMatchingInitData = sessionData.matchingInitData;
        currentMatchingInitData.matching = data.matching;

        var updatedData =
            matchingInitializationService.createMatching(currentMatchingInitData, sessionData.User.UserId);
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
       var data = sessionData.matchingInitData;
        //var data = buffer;


        List<StudentInitDto> students = data.studentRecords;
        var package = documentsProcessingService.formStudentDataReport(students);
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