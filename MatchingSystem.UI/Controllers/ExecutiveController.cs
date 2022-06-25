using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Interface;
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
            data.CurrentStage = matchingRepository.GetCurrentStage(data.SelectedMatching);
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/General/Index.cshtml", data.User);
        }

        [HttpGet]
        public IActionResult Quotas()
        {
            ExecutiveQuotaViewModel model = new ExecutiveQuotaViewModel();

            model.Requests = executiveRepository.GetQuotaRequestsByExecutive(data.User.UserId, data.SelectedMatching);
            model.History = executiveRepository.GetQuotaRequestHistoryByExecutive(data.User.UserId, data.SelectedMatching);

            return View(model);
        }

        [HttpGet]
        public IActionResult Admin()
        {
            data.CurrentStage = matchingRepository.GetCurrentStage(data.SelectedMatching);
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