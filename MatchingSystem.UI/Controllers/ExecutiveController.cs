using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.Service.Executive;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Executive")]
    public class ExecutiveController : Controller
    {
        private readonly IMatchingRepository matchingRepository;
        private readonly ICommonQuotaRepository commonQuotaRepository;
        private readonly IExecutiveServiceOld executiveService;
        private SessionData data;

        public ExecutiveController(IMatchingRepository matchingRepository, IExecutiveServiceOld executiveService, ICommonQuotaRepository commonQuotaRepository)
        {
            this.matchingRepository = matchingRepository;
            this.executiveService = executiveService;
            this.commonQuotaRepository = commonQuotaRepository;
        }
        
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (data == null)
            {
                data = HttpContext.Session.Get<SessionData>("Data");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            if (data == null) return;
            data.CurrentStage = matchingRepository.GetCurrentStageDto(data.SelectedMatching);
            HttpContext.Session.Set("Data", data);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/General/Index.cshtml", data.UserDto);
        }

        [HttpGet]
        public IActionResult Quotas()
        {
            ExecutiveQuotaViewModel model = new ExecutiveQuotaViewModel();
            model.Requests = commonQuotaRepository.GetQuotaRequestsByExecutive(data.UserDto.UserId, data.SelectedMatching);
            model.History = executiveService.GetQuotaRequestHistoryByExecutive(data.UserDto.UserId, data.SelectedMatching);
            return View(model);
        }

        [HttpGet]
        public IActionResult Admin()
        {
            data.CurrentStage = matchingRepository.GetCurrentStageDto(data.SelectedMatching);
            return View();
        }
        
        [HttpGet]
        public ViewResult Adjustment()
        {
            return View();
        }
        [HttpGet]
        public ViewResult matchingInitialize()
        {
            return View();
        }
        [HttpGet]
        public ViewResult monitoringTutors()
        {
            return View();
        }
        [HttpGet]
        public ViewResult monitoringStudents()
        {
            return View();
        }
    }
}