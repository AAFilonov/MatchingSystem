using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Interface;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController: Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IDictionaryRepository dictionaryRepository;
        private readonly IMatchingRepository matchingRepository;
        private SessionData data;
        private Student student;

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            data ??= HttpContext.Session.Get<SessionData>("Data");
            var studentId = studentRepository.GetStudentId(data.User.UserId, data.SelectedMatching);
            student = studentRepository.GetStudent(studentId);
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            data.CurrentStage = matchingRepository.GetCurrentStage(data.SelectedMatching);
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        public StudentController(IStudentRepository studentRepository, IDictionaryRepository dictionaryRepository, IMatchingRepository matchingRepository)
        {
            this.studentRepository = studentRepository;
            this.dictionaryRepository = dictionaryRepository;
            this.matchingRepository = matchingRepository;
        }

        public IActionResult Index()
        {
            return View("~/Views/General/Index.cshtml", data.User);
        }

        public IActionResult Profile()
        {
            StudentViewModel model = new StudentViewModel
            {
                Student = student,
                Technology = dictionaryRepository.GetTechnologiesAll(),
                WorkDirection = dictionaryRepository.GetWorkDirectionsAll()
            };

            return View(model);
        }

        public IActionResult Preferences()
        {
            return View(student.StudentID);
        }
    }
}
