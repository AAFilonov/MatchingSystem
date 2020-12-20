using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Request;
using MatchingSystem.DataLayer.Repository;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IProjectRepository
    {
        //prev: AddProject
        Task CreateProjectAsync(ProjectRequest request);
        Task EditProjectAsync(ProjectRequest request);
        Task DeleteProjectAsync(int projectId);
        Task UpdateProjectQuotaStage3Async(int tutorId, int projectId, int newQty);
        Task UpdateProjectQuotaStage4Async(int tutorId, DataTable projectQuotaTable);
        Task SetProjectCloseAsync(int tutorId, int projectId);
        Task<IEnumerable<Project>> GetProjectsByTutorAsync(int tutorId);
    }
}
