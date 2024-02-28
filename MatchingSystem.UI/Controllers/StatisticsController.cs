using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Executive")]
    [AllArgsConstructor]
    public partial class StatisticsController : Controller
    {
        private readonly IMatchingRepository matchingRepository;
       
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            var data = HttpContext.Session.Get<SessionData>("Data");
            if (data == null)
            {
                data = HttpContext.Session.Get<SessionData>("Data");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            var data = HttpContext.Session.Get<SessionData>("Data");
            data.CurrentStage = StageDto.fromEntity(matchingRepository.GetCurrentStage(data.SelectedMatching));
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = HttpContext.Session.Get<SessionData>("Data");
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