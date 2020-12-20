using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Executive")]
    public class ExecutiveController : Controller
    {
        private readonly DataContext context;
        private SessionData data;

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
            data.CurrentStage = context.GetCurrentStageAsync(data.SelectedMatching).Result;
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        public ExecutiveController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/General/Index.cshtml", data.User);
        }

        [HttpGet]
        public async Task<IActionResult> Quotas()
        {
            ExecutiveQuotaViewModel model = new ExecutiveQuotaViewModel();

            model.Requests = await context.GetQuotaRequestAsync(data.User?.UserID, data.SelectedMatching);
            model.History = await context.GetCommonQuotaHistoryByExecutiveAsync(data.User?.UserID, data.SelectedMatching);
            model.History.Reverse();
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            data.CurrentStage = await context.GetCurrentStageAsync(data.SelectedMatching);
            return View();
        }

        [HttpGet]
        public IActionResult Statistics()
        {
            return View();
        }

        public ViewResult Adjustment()
        {
            return View();
        }
    }
}