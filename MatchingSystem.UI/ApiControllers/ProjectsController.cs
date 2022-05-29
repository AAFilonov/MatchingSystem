using System;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.Service.Projects;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            this.projectsService = projectsService;
        }

        [Route("api/[controller]/tutor/add_project")]
        [HttpPost]
        public IActionResult AddTutorProject([FromForm] ProjectRequest project)
        {
            var result = projectsService.AddTutorProject(project);
            return new JsonResult(result);
        }

        [Route("api/[controller]/tutor/edit_project")]
        [HttpPut]
        public IActionResult EditProject([FromForm] ProjectRequest project)
        {
            projectsService.EditProject(project);
            return Ok();
        }

        [Route("api/[controller]/delete")]
        [HttpDelete]
        public IActionResult DeleteProject([FromQuery] int projectId)
        {
            projectsService.DeleteProject(projectId);
            return NoContent();
        }

        [Route("api/[controller]/get_projects_data")]
        [HttpGet]
        public IActionResult GetProjectsData([FromQuery] int tutorId)
        {
            if (tutorId == default)
            {
                return BadRequest("Отсутствует обязательный параметр.");
            }
            var model = projectsService.GetProjectsData(tutorId);
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
            var matchingId = Convert.ToInt32(data["matching"]);

            projectsService.EditQuota(projectId, quota, tutorId, matchingId);
            return Ok();
        }

        [Route("api/[controller]/closeProject")]
        [HttpPatch]
        public IActionResult CloseProject(int tutorId, int projectId)
        {
            projectsService.CloseProject(tutorId, projectId);
            return Ok();
        }

        [Route("api/[controller]/getProjectsByTutor")]
        [HttpGet]
        public IActionResult GetProjectsByTutor([FromQuery] int tutorId)
        {
            var model = projectsService.GetProjectsByTutor(tutorId);
            return new JsonResult(model);
        }
    }
}