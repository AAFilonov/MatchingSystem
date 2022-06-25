using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.Data.Model;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.Service.DocumentsProcessing;
using MatchingSystem.Service.Exception;
using MatchingSystem.Service.MatchingInitialization;
using MatchingSystem.Service.Monitoring;
using MatchingSystem.Service.Tutor;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace MatchingSystem.UI.ApiControllers;

[ApiController]
public class MonitoringController : ControllerBase
{
    private readonly IMonitoringService monitoringService;

    public MonitoringController(IMonitoringService monitoringService)
    {
        this.monitoringService = monitoringService;
    }

    [Route("api/[controller]/students")]
    [HttpGet]
    public IActionResult getStudentsData()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var result = monitoringService.getMonitoringData(sessionData.SelectedMatching);
        result.studentRecords.ForEach(dto => dto.preferences.Sort((project1, project2) =>
            project1.orderInStudentPrefs.Value.CompareTo(project2.orderInStudentPrefs)));
        return new JsonResult(result);
    }

    [Route("api/[controller]/tutors")]
    [HttpGet]
    public async Task<IActionResult> getTutorsData()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var result = monitoringService.getMonitoringData(sessionData.SelectedMatching);
        result.studentRecords.ForEach(dto => dto.preferences.Sort((project1, project2) =>
            project1.orderInStudentPrefs.Value.CompareTo(project2.orderInStudentPrefs)));
        
        result.tutorRecords.ForEach(tutor => 
            tutor.waitingList.ForEach(dto => 
                dto.preferences.Sort((project1, project2) =>
                    project1.orderInStudentPrefs.Value.CompareTo(project2.orderInStudentPrefs)))
        );
        result.tutorRecords.ForEach(tutor => 
            tutor.waitingList.Sort((student1, student2) =>
                student1.orderInTutorPrefs.Value.CompareTo(student2.orderInTutorPrefs))
        );
        return new JsonResult(result);
    }
}