using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;

namespace Service.Projects;
public interface IProjectsService
{
    public bool AddTutorProject(ProjectRequest project);

    public void EditProject(ProjectRequest project);

    public void DeleteProject(int projectId);

    public TutorProjectsModel GetProjectsData(int tutorId);

    public void EditQuota(int projectId, int quota, int tutorId, int matchingId);

    public void CloseProject(int tutorId, int projectId);

    public IEnumerable<Project> GetProjectsByTutor(int tutorId);
}
