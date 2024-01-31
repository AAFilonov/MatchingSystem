using System.Threading.Tasks;
using MatchingSystem.Service.Monitoring;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;

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
        return new JsonResult(result);
    }

    [Route("api/[controller]/tutors")]
    [HttpGet]
    public async Task<IActionResult> getTutorsData()
    {
        var sessionData = HttpContext.Session.Get<SessionData>("Data");
        var result = monitoringService.getMonitoringData(sessionData.SelectedMatching);
        return new JsonResult(result);
    }
}