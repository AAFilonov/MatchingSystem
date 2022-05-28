using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.Constants;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IDictionaryRepository dictionaryRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IProjectRepository projectRepository;

        public ProjectsController(
            IDictionaryRepository dictionaryRepository, 
            ITutorRepository tutorRepository, 
            IMatchingRepository matchingRepository,
            IProjectRepository projectRepository
            )
        {
            this.dictionaryRepository = dictionaryRepository;
            this.tutorRepository = tutorRepository;
            this.matchingRepository = matchingRepository;
            this.projectRepository = projectRepository;
        }

        [Route("api/[controller]/tutor/add_project")]
        [HttpPost]
        public IActionResult AddTutorProject([FromForm] ProjectRequest project)
        {
            try
            {
                projectRepository.CreateProject(new DataLayer.IO.Params.ProjectParams()
                {
                    Info = project.Info,
                    Quota = (project.Quota=="Не важно")?null:project.Quota,
                    TutorId = project.TutorId,
                    ProjectName = project.Name,
                    CommaSeparatedTechList = string.Join(',', project.TechnologyList ?? new [] { string.Empty }),
                    CommaSeparatedWorkList = string.Join(',', project.WorkDirection ?? new[] { string.Empty }),
                    CommaSeparatedGroupList = string.Join(',', project.AviableGroups ?? new[] { string.Empty })
                });
            }
            catch (Exception)
            {
                return Problem("Возникла неизвестная ошибка", statusCode: 500);
            }

            return new JsonResult(true);
        }

        [Route("api/[controller]/tutor/edit_project")]
        [HttpPut]
        public IActionResult EditProject([FromForm] ProjectRequest project)
        {
            try
            {
                projectRepository.EditProject(new DataLayer.IO.Params.ProjectParams()
                {
                    Info = project.Info,
                    Quota = (project.Quota=="Не важно")?null:project.Quota,
                    TutorId = project.TutorId,
                    ProjectName = project.Name,
                    CommaSeparatedTechList = string.Join(',', project.TechnologyList ?? new [] { string.Empty }),
                    CommaSeparatedWorkList = string.Join(',', project.WorkDirection ?? new[] { string.Empty }),
                    CommaSeparatedGroupList = string.Join(',', project.AviableGroups ?? new[] { string.Empty }),
                    ProjectId = project.ProjectId
                });
                
                return Ok();
            }
            catch (Exception e)
            {
                return Problem("Возникла неизвестная ошибка: "+e.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/delete")]
        [HttpDelete]
        public IActionResult DeleteProject([FromQuery] int projectId)
        {
            try
            {
                projectRepository.DeleteProject(projectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/get_projects_data")]
        [HttpGet]
        public IActionResult GetProjectsData([FromQuery] int tutorId)
        {
            if (tutorId == default)
            {
                return BadRequest("Отсутствует обязательный параметр.");
            }
            
            var model = new TutorProjectsModel
            {
                Projects = projectRepository.GetProjectsByTutor(tutorId).ToList(),
                Groups = tutorRepository.GetGroupsByTutor(tutorId),
                Technology = dictionaryRepository.GetTechnologiesAll(),
                WorkDirections = dictionaryRepository.GetWorkDirectionsAll(),
                CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId),
                IsReady = tutorRepository.GetReadyByTutor(tutorId)
            };

            model.Projects.ForEach(x =>
                x.AvailableGroupsName_List = x.AvailableGroupsName_List?.Replace(", ", "<br/>")
            );

            return new JsonResult(model);
        }

        [Route("api/[controller]/editQuotaPerProject")]
        [HttpPatch]
        public IActionResult EditQuota()
        {
            var data = Request.Form;
            
            var projectId = Convert.ToInt32(data["projectId"]);
            var quota = Convert.ToInt32(data["quota"]);
            var tutorId = Convert.ToInt32(data["tutorId"]);
            

            var currentStageCode = matchingRepository.GetCurrentStage(Convert.ToInt32(data["matching"])).StageTypeCode;

            try
            {
                switch (currentStageCode)
                {
                    case StageCode.CollectStudentPreferences:
                        projectRepository.UpdateProjectQuotaStage3(tutorId, projectId, quota);
                        break;
                    case StageCode.CollectTutorPreferences:
                        projectRepository.UpdateProjectQuotaStage4(tutorId, projectId, (short)quota);
                        break;
                    default:
                        return BadRequest("На данном этапе запрещено изменять проекты");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            
        }

        [Route("api/[controller]/closeProject")]
        [HttpPatch]
        public IActionResult CloseProject(int tutorId, int projectId)
        {
            try
            {
                projectRepository.SetProjectClose(tutorId, projectId);
                return Ok();
            }
            catch (SqlException ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [Route("api/[controller]/getProjectsByTutor")]
        [HttpGet]
        public IActionResult GetProjectsByTutor([FromQuery] int tutorId)
        {
            var model = projectRepository.GetProjectsByTutor(tutorId);
            
            return new JsonResult(model);
        }
    }
}