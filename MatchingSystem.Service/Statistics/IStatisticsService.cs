using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics;
public interface IStatisticsService
{
    public IEnumerable<StatisticsMain> GetStatisticsMain(int? matchingId,int? currentStage);
    public IEnumerable<StatisticsMain> GetStatisticsGroups(int? matchingId);
    
    public IEnumerable<StatisticsTutors> GetStatisticsTutors(int? matchingId, int? currentStage);

    public IEnumerable<StatisticsStudents> GetStatisticsStudents(int? matchingId, int? currentStage);

    public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorsProjectAllocated(int? matchingId, int? tutorId);

    public IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int? matchingId, int? studentId);

    public IEnumerable<Project> GetStatisticsTutorsProjects(int? matchingId, int? tutorId);
}
