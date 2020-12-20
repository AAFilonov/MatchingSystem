using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using MatchingSystem.UI.RequestModels;
using MatchingSystem.UI.ResultModels;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DataContext context;
        private readonly DictionaryRepository dictionaryRepository;

        public ProjectsController(DataContext ctx, IDictionaryRepository dicRepo)
        {
            context = ctx;
            dictionaryRepository = (DictionaryRepository) dicRepo;
        }

        [Route("api/[controller]/tutor/add_project")]
        [HttpPost]
        public async Task<IActionResult> AddTutorProject([FromForm] ProjectRequest project)
        {
            try
            {
                await context.AddProject(
                    project.tutorId,
                    project.name,
                    project.info ?? String.Empty,
                    string.Join(',', project.technologyList ?? new [] { string.Empty }),
                    string.Join(',', project.workDirection ?? new[] { string.Empty }),
                    string.Join(',', project.aviableGroups ?? new[] { string.Empty }),
                    project.quota
                );
            }
            catch (Exception)
            {
                return Problem(detail: "Возникла неизвестная ошибка", statusCode: 500);
            }

            return new JsonResult(true);
        }

        [Route("api/[controller]/tutor/edit_project")]
        [HttpPut]
        public async Task<IActionResult> EditProject([FromForm] ProjectRequest project)
        {
            try
            {
                await context.EditProject(
                    project.projectId.Value,
                    project.tutorId,
                    project.name ?? string.Empty,
                    project.info ?? string.Empty,
                    string.Join(',', project.technologyList ?? new [] { string.Empty }),
                    string.Join(',', project.workDirection ?? new [] { string.Empty }),
                    string.Join(',', project.aviableGroups ?? new [] { string.Empty }),
                    project.quota
                );
                return Ok();
            }
            catch (Exception)
            {
                return Problem(detail: "Возникла неизвестная ошибка", statusCode: 500);
            }
        }

        [Route("api/[controller]/delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProject([FromQuery] int? projectId)
        {
            if (!projectId.HasValue) return Problem(detail: "Отсутствует обязательный параметр.", statusCode: 400);
            try
            {
                //TODO DeleteProjectAsync
                await context.Database.ExecuteSqlRawAsync("exec napp.del_Project null, @ProjectID",
                    new SqlParameter("@ProjectID", projectId));
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/get_projects_data")]
        [HttpGet]
        public async Task<IActionResult> GetProjectsData([FromQuery] int? tutorId)
        {
            if (!tutorId.HasValue) return Problem(detail: "Отсутствует обязательный параметр.", statusCode: 400);
            
            TutorProjectsModel model = new TutorProjectsModel();

            model.Projects = await context.GetTutorProjectsAsync(tutorId);
            model.Projects.ForEach(x => { x.AvailableGroupsName_List = x.AvailableGroupsName_List?.Replace(", ", "<br/>"); });
            
            model.Groups = await context.GetGroupsByTutorAsync(tutorId);

            model.Technology = await dictionaryRepository.GetTechnologiesAllAsync();
            model.WorkDirections = await dictionaryRepository.GetWorkDirectionsAllAsync();
            model.CommonQuota = await context.GetCommonQuotaAsync(tutorId);

            model.IsReady = await context.GetReady(tutorId);

            return new JsonResult(model);
        }

        [Route("api/[controller]/editQuotaPerProject")]
        [HttpPatch]
        public async Task<IActionResult> EditQuota()
        {
            var data = await Request.ReadFormAsync();
            var tutorId = new SqlParameter("@TutorID", Convert.ToInt32(data["tutorId"]));

            var currentStageCode =
                (await context.GetCurrentStageAsync(Convert.ToInt32(data["matching"]))).StageTypeCode;

            if (currentStageCode == 3)
            {
                try
                {
                    //TODO UpdateProjectQuotaStage3Async
                    await context.Database.ExecuteSqlRawAsync(
                        "exec napp.upd_ProjectQuota_ForStage3 @TutorID, @ProjectID, @NewQuotaQty",
                        tutorId,
                        new SqlParameter("@ProjectID", Convert.ToInt32(data["projectId"])),
                        new SqlParameter("@NewQuotaQty", Convert.ToInt32(data["quota"])));
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return Problem(detail: ex.Message);
                }
            }
            else if (currentStageCode == 4)
            {
                try
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("ProjectID", type: typeof(int));
                    dataTable.Columns.Add("Quota", type: typeof(short));

                    var row = dataTable.NewRow();
                    row.SetField(columnName: "ProjectID", Convert.ToInt32(data["projectId"]));
                    row.SetField(columnName: "Quota", Convert.ToInt32(data["quota"]));
                    dataTable.Rows.Add(row);

                    var projectQuota = new SqlParameter("@ProjectQuota", dataTable) {TypeName = "dbo.ProjectQuota"};

                    //TODO UpdateProjectQuotaStage4Async
                    await context.Database.ExecuteSqlRawAsync(
                        "execute napp.upd_ProjectsQuota_ForStage4 @TutorID, @ProjectQuota",
                        tutorId,
                        projectQuota);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return Problem(detail: ex.Message, statusCode: 500);
                }
            }

            return Ok();
        }

        [Route("api/[controller]/closeProject")]
        [HttpPatch]
        public async Task<IActionResult> CloseProject([FromQuery] int? tutorId, [FromQuery] int? projectId)
        {
            if (tutorId == null || projectId == null) return BadRequest("Отсутствует требуемый HTTP параметр");

            try
            {
                //TODO SetProjectCloseAsync
                await context.Database.ExecuteSqlRawAsync("exec napp.upd_Project_Close @TutorID, @ProjectID",
                    new SqlParameter("@TutorID", tutorId),
                    new SqlParameter("@ProjectID", projectId));

                return Ok();
            }
            catch (SqlException ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/getProjectsByTutor")]
        [HttpGet]
        public async Task<IActionResult> GetProjectsByTutor([FromQuery] int? tutorId)
        {
            if (!tutorId.HasValue) return Problem(detail: "Отсутствует обязательный параметр.", statusCode: 400);
            var model = await context.GetTutorProjectsAsync(tutorId);
            return new JsonResult(model);
        }
    }
}