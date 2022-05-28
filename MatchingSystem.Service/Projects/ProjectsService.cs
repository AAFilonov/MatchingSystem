using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.Constants;

namespace Service.Projects;
public class ProjectsService : IProjectsService
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

    public void AddTutorProject( ProjectRequest project)
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
    }

    public void EditProject(ProjectRequest project)
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

    public void DeleteProject(int projectId)
    {
        projectRepository.DeleteProject(projectId);
    }

    public TutorProjectsModel GetProjectsData(int tutorId)
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

        return model;
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

    public IEnumerable<Project> GetProjectsByTutor(int tutorId)
    {
        var model = projectRepository.GetProjectsByTutor(tutorId);

        return model;
    }
}
