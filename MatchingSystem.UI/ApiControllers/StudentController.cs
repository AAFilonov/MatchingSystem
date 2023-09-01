using System;
using System.Linq;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.Service.Follow;
using MatchingSystem.Service.Student;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly IStageTransitionService stageTransitionService;

        public StudentController(IStudentService studentService,IStageTransitionService stageTransitionService)
        {
            this.studentService = studentService;
            this.stageTransitionService = stageTransitionService;
        }

        [Route("api/[controller]/get_selected_info")]
        [HttpGet]
        public IActionResult getStudentInfo(int studentId)
        {
            var model = studentService.getStudentInfo(studentId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/edit_profile")]
        [HttpPatch]
        public IActionResult EditProfile()
        {
            var data = Request.Form;
            
            var editParams = new EditProfileParams()
            {
                StudentId = Convert.ToInt32(data["studentId"]),
                Info = data["info"].ToString(),
                Info2 = data["info2"].ToString(),
                TechnologyCodeList = data["tech"].ToString(),
                WorkDirectionCodeList = data["workDirection"].ToString()
            };
            studentService.EditProfile(editParams);

            return NoContent();
        }

        [Route("api/[controller]/get_projects")]
        [HttpGet]
        public IActionResult GetProjects([FromQuery] int studentId)
        {
            var model = studentService.GetProjects(studentId);

            return new JsonResult(model);
        }

        [Route("api/[controller]/set_preferences")]
        [HttpPost]
        public IActionResult SetPreferences()
        {
            var data = Request.Form;

            var studentId = Convert.ToInt32(data["studentId"]);

            var temp = data["selectedList"].ToString();
            
            studentService.SetPreferences(studentId, temp);
            
            var sessionData = HttpContext.Session.Get<SessionData>("Data"); 
            var currentMatchingId = sessionData .SelectedMatching;
            var need = stageTransitionService.isNeedToTransit(currentMatchingId);
            if (need)
                stageTransitionService.TransitionIfExistNeed(currentMatchingId);

            return Ok();
        }

        [Route("api/[controller]/getStudentInfo")]
        [HttpGet]
        public IActionResult GetStudentInfo([FromQuery] int? studentId)
        {
            if (studentId == null)
            {
                return BadRequest("В запросе отстутствует параметр StudentID");
            }
            var student = studentService.GetStudentInfo(studentId);

            return new JsonResult(student);
        }

        [Route("api/[controller]/getAllocatedProject")]
        [HttpGet]
        public IActionResult GetAllocatedProject([FromQuery] int? studentId)
        {
            if (!studentId.HasValue)
            {
                return BadRequest("Некорректный запрос.");
            }
            var model = studentService.GetAllocatedProject(studentId);
            
            return new JsonResult(model);
        }
    }
}