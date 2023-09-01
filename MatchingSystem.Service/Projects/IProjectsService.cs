using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Projects;
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
