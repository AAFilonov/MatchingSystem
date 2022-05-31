

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.Service;
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
    private ITutorService tutorService;
    private IDocumentsProcessingService documentsProcessingService;
    private ILogger<MatchingInitializationController> logger;

    public MatchingInitializationController(ITutorService service, ILogger<MatchingInitializationController> logger, IDocumentsProcessingService documentsProcessingService)
    {
        this.tutorService = service;
        this.logger = logger;
        this.documentsProcessingService = documentsProcessingService;
    }

    [Route("api/[controller]/create_matching")]
    [HttpGet]
    public IActionResult createNewMatching()
    {
        var result = "HelloWorld";
        return new JsonResult(result);
    }

    [Route("api/[controller]/upload_student_data")]
    [HttpPost]
    public async Task<IActionResult> uploadStudentsDataAsync(IFormCollection files)
    {
        var data = files.Files[0];
        logger.LogInformation("INFO: Accepting student data upload request. Filename is {}",data.FileName);
        
        using (var memoryStream = new MemoryStream())
        {
            await data.CopyToAsync(memoryStream);
            ExcelPackage package = new ExcelPackage(memoryStream);
            documentsProcessingService.parseStudentData(package);
        }

        return new JsonResult( files);
    }
    
    [Route("api/[controller]/get_tutors")]
    [HttpGet]
    public IActionResult getTutors()
    {
        var data = HttpContext.Session.Get<SessionData>("Data");
        var tutorDtos = data.matchingInitData.tutorDtos;
        if (tutorDtos == null)
        {
            tutorDtos = tutorService.GetAllTutors();
        }

        TutorsResponce responce = new TutorsResponce();
        responce.tutors = tutorDtos;
        return new JsonResult(responce);
    }

    [Route("api/[controller]/get_students")]
    [HttpGet]
    public FileResult  getStudents()
    {
        var data = HttpContext.Session.Get<SessionData>("Data");
        List<StudentDto> students = new List<StudentDto>()
        {
            new StudentDto("18ИВТ-1", "Иван", "Иванович", "Иванов", "sa35g7G57"),
            new StudentDto("18ИВТ-1", "Иван", "Иванович", "Иванов", "sa35g7G57"),
        };
        var package =  documentsProcessingService.formStudentDataReport(students);
        
        
       // byte[] a = package.GetAsByteArray();
       var path = AppDomain.CurrentDomain.BaseDirectory;
        package.SaveAs(new FileInfo(path+"/test.xlsx"));

       // return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");  
       return null;
    }
    public class TutorsResponce
    {
        public IEnumerable<TutorDto> tutors { get; set; }
    }
}