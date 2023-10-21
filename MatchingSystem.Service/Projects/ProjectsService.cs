using System.Collections.Generic;
using System.Linq;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Dictionary.repo;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Projects;

public class ProjectsService : IProjectsService
{
    private readonly IDictionaryRepository dictionaryRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IProjectRepository projectRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly IGroupRepository groupRepository;
    private readonly CommonQuotaRepository commonQuotaRepository;

    public ProjectsService(
        IDictionaryRepository dictionaryRepository,
        ITutorRepository tutorRepository,
        IMatchingRepository matchingRepository,
        IProjectRepository projectRepository, CommonQuotaRepository commonQuotaRepository, IGroupRepository groupRepository)
    {
        this.dictionaryRepository = dictionaryRepository;
        this.tutorRepository = tutorRepository;
        this.matchingRepository = matchingRepository;
        this.projectRepository = projectRepository;
        this.commonQuotaRepository = commonQuotaRepository;
        this.groupRepository = groupRepository;
    }

    public bool AddTutorProject(ProjectRequest project)
    {
        projectRepository.CreateProject(new ProjectParams
        {
            Info = project.Info,
            Quota = project.Quota == "Не важно" ? null : project.Quota,
            TutorId = project.TutorId,
            ProjectName = project.Name,
            CommaSeparatedTechList = string.Join(',', project.TechnologyList ?? new[] { string.Empty }),
            CommaSeparatedWorkList = string.Join(',', project.WorkDirection ?? new[] { string.Empty }),
            CommaSeparatedGroupList = string.Join(',', project.AviableGroups ?? new[] { string.Empty })
        });
        return true;
    }

    public void EditProject(ProjectRequest project)
    {
        projectRepository.EditProject(new ProjectParams
        {
            Info = project.Info,
            Quota = project.Quota == "Не важно" ? null : project.Quota,
            TutorId = project.TutorId,
            ProjectName = project.Name,
            CommaSeparatedTechList = string.Join(',', project.TechnologyList ?? new[] { string.Empty }),
            CommaSeparatedWorkList = string.Join(',', project.WorkDirection ?? new[] { string.Empty }),
            CommaSeparatedGroupList = string.Join(',', project.AviableGroups ?? new[] { string.Empty }),
            ProjectId = project.ProjectId
        });
    }

    public void DeleteProject(int projectId)
    {
        projectRepository.DeleteProject(projectId);
    }

    public TutorProjectsModel GetProjectsData(int tutorId)
    {
        var model = new TutorProjectsModel
        {
            Projects = projectRepository.GetProjectsByTutor(tutorId).ToList(),
            Groups = groupRepository.GetGroupsByTutor(tutorId),
            Technology = dictionaryRepository.GetTechnologiesAll(),
            WorkDirections = dictionaryRepository.GetWorkDirectionsAll(),
            CommonQuota = commonQuotaRepository.find( entity =>entity.TutorId== tutorId).First().Qty,
            IsReady = tutorRepository.findById(tutorId).IsReadyToStart
        };

        model.Projects.ForEach(x =>
            x.AvailableGroupsName_List = x.AvailableGroupsName_List?.Replace(", ", "<br/>")
        );

        return model;
    }

    public void EditQuota(int projectId, int quota, int tutorId, int matchingId)
    {
        var currentStageCode = matchingRepository.GetCurrentStageDto(matchingId).StageTypeCode;

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

    public IEnumerable<ProjectDto> GetProjectsByTutor(int tutorId)
    {
        var model = projectRepository.GetProjectsByTutor(tutorId);

        return model;
    }
}