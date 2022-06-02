

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.Service;
using MatchingSystem.Service.DocumentsProcessing;
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
    private readonly IDocumentsProcessingService documentsProcessingService;
    private readonly IMatchingInitializationService matchingInitializationService;
    private readonly ILogger<MatchingInitializationController> logger;


    public MatchingInitializationController(ITutorService service, ILogger<MatchingInitializationController> logger, IDocumentsProcessingService documentsProcessingService, IMatchingInitializationService matchingInitializationService)
    {
        this.tutorService = service;
        this.logger = logger;
        this.documentsProcessingService = documentsProcessingService;
        this.matchingInitializationService = matchingInitializationService;
    }

    [Route("api/[controller]/create_matching")]
    [HttpPost]
    public IActionResult createNewMatching([FromBody] MatchingInitData data)
    {
        var result = "HelloWorld";
        matchingInitializationService.createMatching(data);
        //TODO создать новое распределение и перейти в личный кабинет по нему             
        return Ok();  
    }

    [Route("api/[controller]/upload_student_data")]
    [HttpPost]
    public async Task<IActionResult> uploadStudentsDataAsync(IFormCollection files)
    {
        var data = files.Files[0];
      
        logger.LogInformation("INFO: Accepting student data upload request. Filename is {}",data.FileName);

        await using var memoryStream = new MemoryStream();
        await data.CopyToAsync(memoryStream);
       var sessionData = HttpContext.Session.Get<SessionData>("Data");
        
        ExcelPackage package = new ExcelPackage(memoryStream);
        var groups = documentsProcessingService.parseGroupData(package);
        var students =  documentsProcessingService.parseStudentData(package);

        sessionData.matchingInitData.studentRecords = students;
        sessionData.matchingInitData.groupRecords = groups;
        var tutorDtos = tutorService.GetAllTutors();
        tutorDtos.ToList().ForEach(dto => dto.groups=groups);
        sessionData.matchingInitData.tutorRecords = tutorDtos;
            
        HttpContext.Session.Set<SessionData>("Data", sessionData);
        logger.LogInformation("INFO: Student data upload for file {} was successful. Session context was updated",data.FileName);
        return new JsonResult( students);
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
        public IEnumerable<TutorDto> tutors { get; set; }
    }
}