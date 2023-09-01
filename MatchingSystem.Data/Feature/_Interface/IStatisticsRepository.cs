using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Interface
{
    public interface IStatisticsRepository
    {
      

        //prev: GetMainStatisticsAsync
        Task<IEnumerable<StatisticsMain>> GetStatisticsMainAsync(int matchingId, int currentStage);
        IEnumerable<StatisticsMain> GetStatisticsMain(int matchingId, int currentStage);
        
        Task<IEnumerable<StatisticsTutors>> GetStatisticsTutorsAsync(int matchingId, int currentStage);
        IEnumerable<StatisticsTutors> GetStatisticsTutors(int matchingId, int currentStage);
        
        IEnumerable<Project> GetStatisticsTutorsProjects(int matchingId, int studentId);
        Task<IEnumerable<Project>> GetStatisticsTutorsProjectsAsync(int matchingId, int studentId);
        
        IEnumerable<StatisticsStudents> GetStatisticsStudents(int matchingId, int currentStage);
        Task<IEnumerable<StatisticsStudents>> GetStatisticsStudentsAsync(int matchingId, int currentStage);
        
        IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int matchingId, int studentId);
        Task<IEnumerable<StatisticsStudentsProjects>> GetStatisticsStudentsProjectsAsync(int matchingId, int studentId);
        
        Task<IEnumerable<StatisticsMain>> GetStatisticsGroupsAsync(int matchingId);
         IEnumerable<StatisticsMain> GetStatisticsGroups(int matchingId);
        
        public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorProjectAllocated(int matchingId, int tutorId);
        Task<IEnumerable<TutorsProjectAllocation>> GetStatisticsTutorProjectAllocatedAsync(int matchingId, int tutorId);
    }
}
