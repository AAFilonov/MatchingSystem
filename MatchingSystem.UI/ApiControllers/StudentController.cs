using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using MatchingSystem.UI.ResultModels;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DataContext context;
        private readonly StudentRepository _studentRepository;

        public StudentController(DataContext ctx, IStudentRepository repo)
        {
            _studentRepository = repo as StudentRepository;
            context = ctx;
        }

        [Route("api/[controller]/get_selected_info")]
        [HttpGet]
        public async Task<IActionResult> GetSelectedParams([FromQuery] int? studentId)
        {
            if (!studentId.HasValue) return Problem(detail: "Отсутствует обязательный параметр.", statusCode: 400);
            EditProfile model = new EditProfile();
            Student student = await _studentRepository.GetStudentAsync(studentId.Value);

            SqlParameter studentID = new SqlParameter("@StudentID", studentId);

            try
            {
                model.Technologies = await context.Technologies
                    .FromSqlRaw("select TechnologyCode, TechnologyName_ru from napp.get_Technologies_WithSelected_ByStudent(@StudentID) where IsSelectedByStudent = 1", studentID).ToListAsync();
                model.WorkDirections = await context.WorkDirections
                    .FromSqlRaw("select DirectionCode, DirectionName_ru from napp.get_WorkDirections_WithSelected_ByStudent(@StudentID) where IsSelectedByStudent = 1", studentID).ToListAsync();
                model.Info = student.Info;
                model.Info2 = student.Info2;

                return new JsonResult(model);
            } catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [Route("api/[controller]/edit_profile")]
        [HttpPatch]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var data = await Request.ReadFormAsync();

                SqlParameter studentId = new SqlParameter("@StudentID", Convert.ToInt32(data["studentId"]));
                SqlParameter info = new SqlParameter("@Info", data["info"].ToString());
                SqlParameter info2 = new SqlParameter("@Info2", data["info2"].ToString());
                SqlParameter techList = new SqlParameter("@Technology_CodeList", data["tech"].ToString());
                SqlParameter workList = new SqlParameter("@WorkDirection_CodeList", data["workDirection"].ToString());

            
                await context.Database.ExecuteSqlRawAsync("exec napp.upd_Student_Info " +
                    "@StudentID, " +
                    "@Info, " +
                    "@Info2, " +
                    "@Technology_CodeList, " +
                    "@WorkDirection_CodeList", studentId, info, info2, techList, workList);
                return NoContent();
            } catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [Route("api/[controller]/get_projects")]
        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] int studentId)
        {
            List<ProjectForStudent> model;

            model = await context.StudentProjects
                .FromSqlRaw("select " +
                "ProjectID, TutorNameAbbreviation, ProjectName, Info, TechnologiesName_List, WorkDirectionsName_List, IsSelectedByStudent, OrderNumber " +
                "from napp.get_Projects_ByStudent(@StudentID)", new SqlParameter("@StudentID", studentId))
                .ToListAsync();

            var result = model.OrderBy(x => x.OrderNumber);
            return new JsonResult(result);
        }

        [Route("api/[controller]/set_preferences")]
        [HttpPost]
        public async Task<IActionResult> SetPreferences()
        {
            var data = await Request.ReadFormAsync();

            var temp = data["selectedList"].ToString().Split(',');
            int[] selectedIds = new int[temp.Length];

            SqlParameter studentID = new SqlParameter("@StudentID", Convert.ToInt32(data["studentId"]));

            try
            {
                await context.Database.ExecuteSqlRawAsync("exec napp.del_StudentsPreferences @StudentID", studentID);
            } catch (Exception ex)
            {
                return Problem(detail: "Произошла ошибка при изменении списка предпочтений: " + ex.Message);
            }

            for (int i = 0; i < temp.Length; i++)
            {
                selectedIds[i] = Convert.ToInt32(temp[i]);
                
                try
                {
                    await context.Database
                        .ExecuteSqlRawAsync("exec napp.create_StudentsPreference @StudentID, @ProjectID, @OrderNumber",
                        studentID,
                        new SqlParameter("@ProjectID", selectedIds[i]),
                        new SqlParameter("@OrderNumber", i + 1));
                } catch(Exception ex)
                {
                    return Problem(detail: "Произошла ошибка при изменении списка предпочтений: " + ex.Message);
                }
            }      
            return Ok();
        }
        
        [Route("api/[controller]/getStudentInfo")]
        [HttpGet]
        public async Task<IActionResult> GetStudentInfo([FromQuery] int? studentId)
        {
            if (studentId == null) return BadRequest("В запросе отстутствует параметр StudentID");
            Student student = await _studentRepository.GetStudentAsync(studentId.Value);

            return  new JsonResult(student);
        }

        [Route("api/[controller]/getAllocatedProject")]
        [HttpGet]
        public async Task<IActionResult> GetAllocatedProject([FromQuery] int? studentId)
        {
            if (!studentId.HasValue) return Problem(detail: "Некорректный запрос.", statusCode: 400);
            var model = await context.GetAllocationByStudentAsync(studentId);
            return new JsonResult(model);
        }
    }
}