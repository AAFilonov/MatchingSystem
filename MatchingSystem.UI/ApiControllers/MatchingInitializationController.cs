using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.MatchingInit;
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


    public MatchingInitializationController(ITutorService tutorService, ILogger<MatchingInitializationController> logger, IMatchingInitializationService matchingInitializationService, ITutorsParsingService tutorsParsingService, IStudentsParsingService studentsParsingService)
    {
        this.tutorService = tutorService;
        this.logger = logger;
        this.matchingInitializationService = matchingInitializationService;
        this.tutorsParsingService = tutorsParsingService;
        this.studentsParsingService = studentsParsingService;
    }

    [Route("api/[controller]/create_matching")]
    [HttpPost]
    public IActionResult createNewMatching([FromBody] MatchingInitData data)
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data"); 
        var currentMatchingInitData = sessionData .matchingInitData;
        currentMatchingInitData.matching = data.matching;
        
        matchingInitializationService.createMatching(currentMatchingInitData,sessionData.User.UserId);
        //TODO создать новое распределение и перейти в личный кабинет по нему             
        return Ok();  
    }

    [Route("api/[controller]/upload_students_data")]
    [HttpPost]  
    public async Task<IActionResult> uploadStudentsDataAsync(IFormCollection files)
    {
        var data = files.Files[0];
      
        logger.LogInformation("INFO: Accepting student data upload request. Filename is {}",data.FileName);

        await using var memoryStream = new MemoryStream();
        await data.CopyToAsync(memoryStream);
       var sessionData = HttpContext.Session.Get<SessionData>("Data");
        
        ExcelPackage package = new ExcelPackage(memoryStream);
        var groups = studentsParsingService.parseStudentGroupData(package);
        var students =  studentsParsingService.tryToParseStudentData(package);

        sessionData.matchingInitData.studentRecords = students;
        sessionData.matchingInitData.groupRecords = groups;
     
        HttpContext.Session.Set("Data", sessionData);
        logger.LogInformation("INFO: Student data upload for file {} was successful. Session context was updated",data.FileName);
        return new JsonResult( students);
    }
    
    [Route("api/[controller]/upload_tutors_data")]
    [HttpPost]
    public async Task<IActionResult> uploadTutorDataAsync(IFormCollection files)
    {
        var data = files.Files[0];
      
        logger.LogInformation("INFO: Accepting tutors data upload request. Filename is {}",data.FileName);

        await using var memoryStream = new MemoryStream();
        await data.CopyToAsync(memoryStream);
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        
        ExcelPackage package = new ExcelPackage(memoryStream);
        var groups = tutorsParsingService.parseTutorGroupData(package);
        bool tutorGroupsMatchWithStudentGroups =
            groups.All(dto => sessionData.matchingInitData.groupRecords.Contains(dto));
        
        if(!tutorGroupsMatchWithStudentGroups)
        {
            throw new InputDataException("Tutor groups does`t match with student groups!");
        }
        
        var tutors =  tutorsParsingService.tryToParseTutorsData(package);
        sessionData.matchingInitData.tutorRecords = tutors;
        
        HttpContext.Session.Set("Data", sessionData);
        logger.LogInformation("INFO: Tutors data upload for file {} was successful. Session context was updated",data.FileName);
        return new JsonResult( tutors);
    }
    
    [Route("api/[controller]/get_tutors")]
    [HttpGet]
    public IActionResult getTutors()
    {
      
        var sessionData = HttpContext.Session.Get<SessionData>("Data");

        var tutorDtos = sessionData.matchingInitData.tutorRecords;
        if (tutorDtos == null)
        {
            tutorDtos = tutorService.GetAllTutors();
            if (sessionData.matchingInitData.groupRecords != null)
                tutorDtos.ToList().ForEach(dto => dto.groups = sessionData.matchingInitData.groupRecords);
        }

        TutorsResponce responce = new TutorsResponce();
        responce.tutors = tutorDtos;
        
        
        return new JsonResult(responce);
    }

    [Route("api/[controller]/matching_data_sync")]
    [HttpGet]
    public IActionResult getMatchingData()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var data = sessionData.matchingInitData;
        return new JsonResult(data);
    }
    [Route("api/[controller]/matching_data_sync")]
    [HttpPost]
    public IActionResult postMatchingData()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var data = sessionData.matchingInitData;
        return new JsonResult(data);
    }
    
    public class TutorsResponce
    {
        public IEnumerable<TutorInitDto> tutors { get; set; }
    }
}