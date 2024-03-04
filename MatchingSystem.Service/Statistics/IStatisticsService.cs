using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Statistics;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics;

public interface IStatisticsService
{
    //TODO DPL-42 написать реализации
    public IEnumerable<StatisticsMain> GetStatisticsMain(int matchingId);
    public IEnumerable<StatisticsMain> GetStatisticsGroups(int? matchingId);

    public IEnumerable<StatisticsTutors> GetStatisticsTutors(int? matchingId, int? currentStage);

    public IEnumerable<StudentStatistics> GetStatisticsStudents(int matchingId);

    public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorsProjectAllocated(int? matchingId, int? tutorId);

    public IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int? matchingId, int? studentId);

    public IEnumerable<ProjectDto> GetStatisticsTutorsProjects(int? matchingId, int? tutorId);
}