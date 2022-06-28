using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.DataLayer.Repository;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IProjectRepository
    {
        Task CreateProjectAsync(ProjectParams @params);
        IEnumerable<TutorInitDto> SetDefaultProjectsForTutors(List<TutorInitDto> tutors, int matchingId);
        void SetDefaultProjects_Groups(List<TutorInitDto> tuts);
        void CreateProject(ProjectParams @params);
        Task EditProjectAsync(ProjectParams @params);
        Task<IEnumerable<ProjectGroupDTO>> GetProjectsGroupsBtMatching(int studentId);
        void EditProject(ProjectParams @params);
        Task DeleteProjectAsync(int projectId);
        void DeleteProject(int projectId);
        Task UpdateProjectQuotaStage3Async(int tutorId, int projectId, int newQty);
        void UpdateProjectQuotaStage3(int tutorId, int projectId, int newQty);
        Task UpdateProjectQuotaStage4Async(int tutorId, int projectId, short projectQuota);
        void UpdateProjectQuotaStage4(int tutorId, int projectId, short projectQuota);
        Task SetProjectCloseAsync(int tutorId, int projectId);
        void SetProjectClose(int tutorId, int projectId);
        Task<IEnumerable<Project>> GetProjectsByTutorAsync(int tutorId);
        IEnumerable<Project> GetProjectsByTutor(int tutorId);
        public IEnumerable<Project> GetProjectsByMatching(int MatchingId);
        Task<IEnumerable<ProjectForStudent>> GetProjectsByStudentAsync(int studentId);
        IEnumerable<ProjectForStudent> GetProjectsByStudent(int studentId);
    }
}
