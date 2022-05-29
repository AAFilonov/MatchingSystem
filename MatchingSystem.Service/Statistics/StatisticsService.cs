using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.Service.Statistics;
public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepository statisticsRepository;
    public StatisticsService(IStatisticsRepository statisticsRepository)
    {
        this.statisticsRepository = statisticsRepository;
    }

    public IEnumerable<StatisticsMain> GetStatisticsMain(int? matchingId,int? currentStage)
    {
        if (currentStage > 4)
        {
            currentStage = 4;
        }

        var result = statisticsRepository.GetStatisticsMain(matchingId.Value, currentStage.Value);

        foreach (var stat in result)
        {
            stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
        }

        return result;
    }

    public IEnumerable<StatisticsMain> GetStatisticsGroups(int? matchingId)
    {
        var result = statisticsRepository.GetStatisticsGroups(matchingId.Value);

        foreach (var stat in result)
        {
            stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
        }

        return result;
    }

    public IEnumerable<StatisticsTutors> GetStatisticsTutors(int? matchingId,int? currentStage)
    {
        if (currentStage > 4)
        {
            currentStage = 4;
        }

        var result = statisticsRepository.GetStatisticsTutors(matchingId.Value, currentStage.Value);

        return result;
    }

    public IEnumerable<StatisticsStudents> GetStatisticsStudents(int? matchingId, int? currentStage)
    {
        if (currentStage > 4)
        {
            currentStage = 4;
        }

        var result = statisticsRepository.GetStatisticsStudents(matchingId.Value, currentStage.Value);

        return result;
    }

    public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorsProjectAllocated(int? matchingId, int? tutorId)
    {
        var result = statisticsRepository.GetStatisticsTutorProjectAllocated(matchingId.Value, tutorId.Value);
        return result;
    }

    public IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int? matchingId, int? studentId)
    {
        var model = statisticsRepository.GetStatisticsStudentsProjects(matchingId.Value, studentId.Value);

        return model.OrderBy(project => project.OrderNumber);
    }

    public IEnumerable<Project> GetStatisticsTutorsProjects(int? matchingId, int? tutorId)
    {
        var result = statisticsRepository.GetStatisticsTutorsProjects(matchingId.Value, tutorId.Value);
        return result;
    }
}