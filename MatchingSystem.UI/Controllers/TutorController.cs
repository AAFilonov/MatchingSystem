using MatchingSystem.DataLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Tutor")]
    public class TutorController : Controller
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IMatchingRepository matchingRepository;
        private SessionData data;
        
        public TutorController(ITutorRepository tutorRepository, IMatchingRepository matchingRepository)
        {
            this.matchingRepository = matchingRepository;
            this.tutorRepository = tutorRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (data != null) return;
            data = HttpContext.Session.Get<SessionData>("Data");
            data.TutorId = tutorRepository.GetTutorId(data.User.UserID, data.SelectedMatching);
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            data.CurrentStage = matchingRepository.GetCurrentStage(data.SelectedMatching);
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["active"] = "index";
            ViewData["title"] = "Личный кабинет";
            return View("~/Views/General/Index.cshtml", data.User);
        }

        public IActionResult Projects()
        {
            ViewData["title"] = "Проекты";
            ViewData["active"] = "projects";
            return View();
        }

        [HttpGet]
        public IActionResult Quota()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Iterations()
        {
            return View();
        }
    }
}
