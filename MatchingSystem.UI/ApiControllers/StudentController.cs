using System;
using System.Linq;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IProjectRepository projectRepository;

        public StudentController(IStudentRepository studentRepository, IProjectRepository projectRepository)
        {
            this.studentRepository = studentRepository;
            this.projectRepository = projectRepository;
        }

        [Route("api/[controller]/get_selected_info")]
        [HttpGet]
        public IActionResult GetSelectedParams(int studentId)
        {
            try
            {
                var student = studentRepository.GetStudent(studentId);
                var model = new GetData()
                {
                    Technologies = studentRepository.GetTechnologiesSelectedByStudent(studentId),
                    WorkDirections = studentRepository.GetWorkDirectionsSelectedByStudent(studentId),
                    Info = student.Info,
                    Info2 = student.Info2
                };

                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Route("api/[controller]/edit_profile")]
        [HttpPatch]
        public IActionResult EditProfile()
        {
            try
            {
                var data = Request.Form;

                studentRepository.EditProfile(new EditProfileParams()
                {
                    StudentId = Convert.ToInt32(data["studentId"]),
                    Info = data["info"].ToString(),
                    Info2 = data["info2"].ToString(),
                    TechnologyCodeList = data["tech"].ToString(),
                    WorkDirectionCodeList = data["workDirection"].ToString()
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Route("api/[controller]/get_projects")]
        [HttpGet]
        public IActionResult GetProjects([FromQuery] int studentId)
        {
            var model = projectRepository.GetProjectsByStudent(studentId);

            return new JsonResult(model.OrderBy(x => x.OrderNumber));
        }

        [Route("api/[controller]/set_preferences")]
        [HttpPost]
        public IActionResult SetPreferences()
        {
            var data = Request.Form;

            var studentId = Convert.ToInt32(data["studentId"]);

            var temp = data["selectedList"].ToString().Split(',');
            var selectedIds = new int[temp.Length];

            try
            {
                studentRepository.ClearPreferences(studentId);

                for (var i = 0; i < temp.Length; i++)
                {
                    selectedIds[i] = Convert.ToInt32(temp[i]);
                    studentRepository.SetPreferences(new StudentPreferenceParams()
                    {
                        StudentId = studentId,
                        Order = i + 1,
                        SelectedProjectId = selectedIds[i]
                    });
                }
            }
            catch (Exception ex)
            {
                return Problem("Произошла ошибка при изменении списка предпочтений: " + ex.Message);
            }

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

            var student = studentRepository.GetStudent(studentId.Value);

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

            var model = studentRepository.GetAllocationByStudent(studentId.Value);
            
            return new JsonResult(model);
        }
    }
}