using MatchingSystem.Constants;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
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
        private readonly IQuotaRequestRepository quotaRequestRepository;

        private SessionData data;

        public ExecutiveController(IMatchingRepository matchingRepository, IQuotaRequestRepository quotaRequestRepository)
        {
            this.matchingRepository = matchingRepository;
            this.quotaRequestRepository = quotaRequestRepository;
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
            data.CurrentStage = StageDto.fromEntity(matchingRepository.GetCurrentStage(data.SelectedMatching));
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
            model.Requests = quotaRequestRepository.GetQuotaRequestsByExecutive(data.UserDto.UserId, data.SelectedMatching);
            model.History = quotaRequestRepository.GetQuotaRequestHistoryByExecutive(data.SelectedMatching);
            return View(model);
        }

        [HttpGet]
        public IActionResult Admin()
        {
            ViewData["version"] = SystemParameters.getInstance().VERSION;
            data.Version = SystemParameters.getInstance().VERSION;
            data.CurrentStage = StageDto.fromEntity(matchingRepository.GetCurrentStage(data.SelectedMatching));
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