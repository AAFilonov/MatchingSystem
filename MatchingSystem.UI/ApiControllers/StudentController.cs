using System;
using System.Linq;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Student;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [Route("api/[controller]/get_selected_info")]
        [HttpGet]
        public IActionResult GetSelectedParams(int studentId)
        {
            var model = studentService.GetSelectedParams(studentId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/edit_profile")]
        [HttpPatch]
        public IActionResult EditProfile()
        {
            var data = Request.Form;

            int StudentId = Convert.ToInt32(data["studentId"]);
            string Info = data["info"].ToString();
            string Info2 = data["info2"].ToString();
            string TechnologyCodeList = data["tech"].ToString();
            string WorkDirectionCodeList = data["workDirection"].ToString();

            studentService.EditProfile(StudentId,Info,Info2, TechnologyCodeList, WorkDirectionCodeList);

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