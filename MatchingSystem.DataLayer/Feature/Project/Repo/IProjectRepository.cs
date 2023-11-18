using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.IO.Params;

namespace MatchingSystem.DataLayer.Feature.Project.Repo
{
    public interface IProjectRepository
    {
        Task CreateProjectAsync(ProjectParams @params);
        IEnumerable<TutorInitDto> CreateDefaultProjectsForTutors(List<TutorInitDto> tutors, int matchingId);
        void SetDefaultProjects_Groups(List<TutorInitDto> tuts);
        void CreateProject(ProjectParams @params);
        void CreateDefaultTutorsProjects(List<TutorInitDto> tuts);
        Task EditProjectAsync(ProjectParams @params);
        void EditProject(ProjectParams @params);
        Task DeleteProjectAsync(int projectId);
        void DeleteProject(int projectId);
        Task UpdateProjectQuotaStage3Async(int tutorId, int projectId, int newQty);
        void UpdateProjectQuotaStage3(int tutorId, int projectId, int newQty);
        Task UpdateProjectQuotaStage4Async(int tutorId, int projectId, short projectQuota);
        void UpdateProjectQuotaStage4(int tutorId, int projectId, short projectQuota);
        Task SetProjectCloseAsync(int tutorId, int projectId);
        void SetProjectClose(int tutorId, int projectId);
        Task<IEnumerable<ProjectDto>> GetProjectsByTutorAsync(int tutorId);
        IEnumerable<ProjectDto> GetProjectsByTutor(int tutorId);
        public IEnumerable<ProjectDto> GetProjectsByMatching(int MatchingId);
        Task<IEnumerable<ProjectForStudentDto>> GetProjectsByStudentAsync(int studentId);
        IEnumerable<ProjectForStudentDto> GetProjectsByStudent(int studentId);
    }
}
