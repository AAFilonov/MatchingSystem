using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Statistics.repo
{
    public interface IStatisticsRepositoryOld
    {
        IEnumerable<StatisticsMain> GetStatisticsMain(int matchingId, int currentStage);
        IEnumerable<StatisticsTutors> GetStatisticsTutors(int matchingId, int currentStage);
        IEnumerable<ProjectDto> GetStatisticsTutorsProjects(int matchingId, int studentId);
        IEnumerable<StudentStatistics> GetStatisticsStudents(int matchingId, int currentStage);
        IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int matchingId, int studentId);
        IEnumerable<StatisticsMain> GetStatisticsGroups(int matchingId);
        public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorProjectAllocated(int matchingId, int tutorId);
  
    }
}
