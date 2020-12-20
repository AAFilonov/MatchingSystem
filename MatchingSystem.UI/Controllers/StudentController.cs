using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController: Controller
    {
        private readonly DataContext context;
        private readonly StudentRepository studentRepository;
        private readonly DictionaryRepository dictionaryRepository;
        private SessionData data;
        private Student student;

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            data ??= HttpContext.Session.Get<SessionData>("Data");
            var studentId = studentRepository.GetStudentId(data.User.UserID, data.SelectedMatching.Value);
            student = studentRepository.GetStudent(studentId);
        }

        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            base.OnActionExecuted(ctx);
            var task = context.GetCurrentStageAsync(data.SelectedMatching);
            task.Wait();
            data.CurrentStage = task.Result;
            HttpContext.Session.Set<SessionData>("Data", data);
        }

        public StudentController(DataContext ctx, IStudentRepository studentRepo, IDictionaryRepository dictionaryRepo)
        {
            context = ctx;
            studentRepository = (StudentRepository)studentRepo;
            dictionaryRepository = (DictionaryRepository)dictionaryRepo;
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
