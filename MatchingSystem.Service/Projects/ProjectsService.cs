using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Dictionary.repo;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;

namespace MatchingSystem.Service.Projects;

[AllArgsConstructor]
public partial class ProjectsService : IProjectsService
{
    private readonly IDictionaryRepository dictionaryRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly ITutorRepository tutorRepository;
    private readonly IGroupRepository groupRepository;
    private readonly ICommonQuotaRepository commonQuotaRepository;
    private readonly IQuotaRequestRepository quotaRequestRepository;
    

    public bool AddTutorProject(ProjectRequest project)
    {
        projectRepositoryOld.CreateProject(new ProjectParams
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
        projectRepositoryOld.EditProject(new ProjectParams
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
        projectRepositoryOld.DeleteProject(projectId);
    }

    public TutorProjectsModel GetProjectsData(int tutorId)
    {
        var model = new TutorProjectsModel();
        model.Projects = projectRepositoryOld.GetProjectsByTutor(tutorId).ToList();
        model.Groups = groupRepository.GetGroupsByTutor(tutorId);
        model.Technology = dictionaryRepository.GetTechnologiesAll();
        model.WorkDirections = dictionaryRepository.GetWorkDirectionsAll();
        model.CommonQuota = quotaRequestRepository.GetActiveTutorQuota(tutorId)?.Qty;
        model.IsReady = tutorRepository.findById(tutorId).IsReadyToStart;

        model.Projects.ForEach(x =>
            x.AvailableGroupsName_List = x.AvailableGroupsName_List?.Replace(", ", "<br/>")
        );

        return model;
    }

    public void EditQuota(int projectId, int quota, int tutorId, int matchingId)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);

        switch (currentStage.StageTypeId)
        {
            case (int)StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES:
                projectRepositoryOld.UpdateProjectQuotaStage3(tutorId, projectId, quota);
                break;
            case (int)StageTypeEnum.ITERATION:
            case (int)StageTypeEnum.MATCHING_PREPARATION:
                projectRepositoryOld.UpdateProjectQuotaStage4(tutorId, projectId, (short)quota);
                break;
        }
    }

    public void CloseProject(int tutorId, int projectId)
    {
        projectRepositoryOld.SetProjectClose(tutorId, projectId);
    }

    public IEnumerable<ProjectDto> GetProjectsByTutor(int tutorId)
    {
        var model = projectRepositoryOld.GetProjectsByTutor(tutorId);

        return model;
    }
}