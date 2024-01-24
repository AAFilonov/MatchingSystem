using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Statistics
{
    public interface IStatisticsRepository
    {
        IEnumerable<StatisticsMain> GetStatisticsMain(int matchingId, int currentStage);
        

        IEnumerable<StatisticsTutors> GetStatisticsTutors(int matchingId, int currentStage);
        
        IEnumerable<ProjectDto> GetStatisticsTutorsProjects(int matchingId, int studentId);

        IEnumerable<StatisticsStudents> GetStatisticsStudents(int matchingId, int currentStage);

        IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int matchingId, int studentId);

        IEnumerable<StatisticsMain> GetStatisticsGroups(int matchingId);
        
        public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorProjectAllocated(int matchingId, int tutorId);
  
    }
}
