using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Executive")]
    [AllArgsConstructor]
    public partial class StatisticsController : Controller
    {
        private readonly IMatchingRepository matchingRepository;
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
            data.CurrentStage = matchingRepository.GetCurrentStageDto(data.SelectedMatching);
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/General/Index.cshtml", data.UserDto);
        }
        
        [HttpGet]
        public IActionResult Main()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Students()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Tutors()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Groups()
        {
            return View();
        }
    }
}