using System.Linq;
using MatchingSystem.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Tutor")]
    public class TutorController : Controller
    {
        private readonly DataContext context;
        private SessionData data;

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (data == null)
            {
                data = HttpContext.Session.Get<SessionData>("Data");
                //TODO GetTutorIdAsync
                data.TutorID = context.GetInt.FromSqlRaw("select napp.get_TutorID(@UserID, @MatchingID) as Value",
                    new SqlParameter("@UserID", data.User.UserID),
                    new SqlParameter("@MatchingID", (int)data.SelectedMatching)).FirstOrDefault().Value;
                HttpContext.Session.Set<SessionData>("Data", data);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            data.CurrentStage = context.GetCurrentStageAsync(data.SelectedMatching).Result;
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        public TutorController(DataContext context)
        {
            this.context = context;
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
