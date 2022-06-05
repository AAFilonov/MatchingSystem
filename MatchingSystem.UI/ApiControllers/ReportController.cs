

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.Service;
using MatchingSystem.Service.DocumentsProcessing;
using MatchingSystem.Service.Tutor;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

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
        
        
        var path = AppDomain.CurrentDomain.BaseDirectory;
        package.SaveAs(new FileInfo(path+"/test.xlsx"));

         return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");  
    }
}