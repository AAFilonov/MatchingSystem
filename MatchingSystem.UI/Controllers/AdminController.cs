using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.Controllers
{
    //[Authorize(Roles = "Admin")] TODO DPL-15 баг авотризации админа
    public class AdminController : Controller
    {
        private readonly IMatchingRepository matchingRepository;
        private SessionData data;

        public AdminController(IMatchingRepository matchingRepository)
        {
            this.matchingRepository = matchingRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (data != null) return;
            data = HttpContext.Session.Get<SessionData>("Data");
            HttpContext.Session.Set("Data", data);
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            data.CurrentStage = StageDto.fromEntity(matchingRepository.GetCurrentStage(data.SelectedMatching));
            HttpContext.Session.Set("Data", data);
        }

        public IActionResult Index()
        {
            ViewData["active"] = "index";
            ViewData["title"] = "Личный кабинет";
            return View("~/Views/General/Index.cshtml", data.UserDto);
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }
    }
}
