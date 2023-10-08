using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Dictionary.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController: Controller
    {
        private readonly IStudentRepositoryOld studentRepositoryOld;
        private readonly IDictionaryRepository dictionaryRepository;
        private readonly IMatchingRepository matchingRepository;
        private SessionData data;
        private StudentDto studentDto;

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            data ??= HttpContext.Session.Get<SessionData>("Data");
            var studentId = studentRepositoryOld.GetStudentId(data.UserDto.UserId, data.SelectedMatching);
            studentDto = studentRepositoryOld.GetStudent(studentId);
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            data.CurrentStage = matchingRepository.GetCurrentStageDto(data.SelectedMatching);
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        public StudentController(IStudentRepositoryOld studentRepositoryOld, IDictionaryRepository dictionaryRepository, IMatchingRepository matchingRepository)
        {
            this.studentRepositoryOld = studentRepositoryOld;
            this.dictionaryRepository = dictionaryRepository;
            this.matchingRepository = matchingRepository;
        }

        public IActionResult Index()
        {
            return View("~/Views/General/Index.cshtml", data.UserDto);
        }

        public IActionResult Profile()
        {
            StudentViewModel model = new StudentViewModel
            {
                studentDto = studentDto,
                Technology = dictionaryRepository.GetTechnologiesAll(),
                WorkDirection = dictionaryRepository.GetWorkDirectionsAll()
            };

            return View(model);
        }

        public IActionResult Preferences()
        {
            return View(studentDto.StudentID);
        }
    }
}
