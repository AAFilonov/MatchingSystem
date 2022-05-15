using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.UI.RequestModels;
using MatchingSystem.Constants;

namespace Service.Projects
{
    internal class ProjectsService
    {

        private readonly IDictionaryRepository dictionaryRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IProjectRepository projectRepository;

        public ProjectsService(
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

        public IActionResult AddTutorProject([FromForm] ProjectRequest project)
        {
            projectRepository.CreateProject(new ProjectParams()
            {
                Info = project.Info,
                Quota = (project.Quota == "Не важно") ? null : project.Quota,
                TutorId = project.TutorId,
                ProjectName = project.Name,
                CommaSeparatedTechList = string.Join(',', project.TechnologyList ?? new[] { string.Empty }),
                CommaSeparatedWorkList = string.Join(',', project.WorkDirection ?? new[] { string.Empty }),
                CommaSeparatedGroupList = string.Join(',', project.AviableGroups ?? new[] { string.Empty })
            });

            return new JsonResult(true);
        }

        public void EditProject([FromForm] ProjectRequest project)
        {
            
            projectRepository.EditProject(new ProjectParams()
            {
                Info = project.Info,
                Quota = (project.Quota == "Не важно") ? null : project.Quota,
                TutorId = project.TutorId,
                ProjectName = project.Name,
                CommaSeparatedTechList = string.Join(',', project.TechnologyList ?? new[] { string.Empty }),
                CommaSeparatedWorkList = string.Join(',', project.WorkDirection ?? new[] { string.Empty }),
                CommaSeparatedGroupList = string.Join(',', project.AviableGroups ?? new[] { string.Empty }),
                ProjectId = project.ProjectId
            });
        }

        public void DeleteProject([FromQuery] int projectId)
        {
            projectRepository.DeleteProject(projectId);
        }

        public IActionResult GetProjectsData([FromQuery] int tutorId)
        {
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

        public void EditQuota(int projectId,int quota,int tutorId,int matchingid)
        {
            var currentStageCode = matchingRepository.GetCurrentStage(matchingid).StageTypeCode;

            switch (currentStageCode)
            {
                case StageCode.CollectStudentPreferences:
                    projectRepository.UpdateProjectQuotaStage3(tutorId, projectId, quota);
                    break;
                case StageCode.CollectTutorPreferences:
                    projectRepository.UpdateProjectQuotaStage4(tutorId, projectId, (short)quota);
                    break;
            }
        }

        public void CloseProject(int tutorId, int projectId)
        {
            projectRepository.SetProjectClose(tutorId, projectId);
        }

        public IActionResult GetProjectsByTutor([FromQuery] int tutorId)
        {
            var model = projectRepository.GetProjectsByTutor(tutorId);

            return new JsonResult(model);
        }
    }
}
