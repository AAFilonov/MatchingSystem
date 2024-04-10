using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using MatchingSystem.Service.DocumentsProcessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatchingSystem.UI.ApiControllers;

[ApiController]
public class ReportController : ControllerBase
{

    private readonly IDocumentsProcessingService documentsProcessingService;
    private readonly ILogger<ReportController> logger;

    public ReportController(IDocumentsProcessingService documentsProcessingService, ILogger<ReportController> logger)
    {
        this.documentsProcessingService = documentsProcessingService;
        this.logger = logger;
    }
    

    [Route("api/[controller]/student_data_report")]
    [HttpGet]
    public FileResult getStudents()
    {
        
        List<StudentInitDto> students = new List<StudentInitDto>()
        {
            new StudentInitDto("18ИВТ-1", "Иван", "Иванович", "Иванов", "sa35g7G57"),
            new StudentInitDto("18ИВТ-1", "Иван", "Иванович", "Иванов", "sa35g7G57"),
        };
        var package =  documentsProcessingService.formStudentDataReport(students);
        

         return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");  
    }
}