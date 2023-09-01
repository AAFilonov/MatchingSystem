using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.Matching.service;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Executive")]
    public class ExecutiveController : Controller
    {
        private readonly IMatchingRepository matchingRepository;
        private readonly IExecutiveRepository executiveRepository;
        private SessionData data;

        public ExecutiveController(IMatchingRepository matchingRepository, IExecutiveRepository executiveRepository)
        {
            this.matchingRepository = matchingRepository;
            this.executiveRepository = executiveRepository;
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

            model.Requests = executiveRepository.GetQuotaRequestsByExecutive(data.UserDto.UserId, data.SelectedMatching);
            model.History = executiveRepository.GetQuotaRequestHistoryByExecutive(data.UserDto.UserId, data.SelectedMatching);

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