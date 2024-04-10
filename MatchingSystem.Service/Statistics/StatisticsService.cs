using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Statistics;
using MatchingSystem.DataLayer.Feature.Statistics.repo;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics;

[AllArgsConstructor]
public partial class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepositoryOld statisticsRepositoryOld;
    private readonly IStatisticsRepository statisticsRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IMainStatisticsService mainStatisticsService;
    
    public IEnumerable<StatisticsMain> GetStatisticsMain(int matchingId)
    {
        var result = mainStatisticsService.GetCurrentStageStatistics(matchingId);
        foreach (var stat in result) stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
        return result;
    }

    public IEnumerable<StatisticsMain> GetStatisticsGroups(int? matchingId) //TODO DPL-42 переписать на EF
    {
        var result = statisticsRepositoryOld.GetStatisticsGroups(matchingId.Value);
        foreach (var stat in result) stat.StatValue_Str = stat.StatValue_Str?.Replace(",", "<br>");
        return result;
    }

    public IEnumerable<StatisticsTutors> GetStatisticsTutors(int? matchingId, int? currentStage) //TODO DPL-42 переписать на EF
    {
        if (currentStage > 4) currentStage = 4;
        var result = statisticsRepositoryOld.GetStatisticsTutors(matchingId.Value, currentStage.Value);
        return result;
    }

    public IEnumerable<StudentStatistics> GetStatisticsStudents(int matchingId)
    {
        int currentStage = matchingRepository.GetCurrentStage(matchingId).StageId;
        return statisticsRepository.GetStatisticsStudents(matchingId,currentStage);
    }

    public IEnumerable<TutorsProjectAllocation> GetStatisticsTutorsProjectAllocated(int? matchingId, int? tutorId) //TODO DPL-42 переписать на EF
    {
        var result = statisticsRepositoryOld.GetStatisticsTutorProjectAllocated(matchingId.Value, tutorId.Value);
        return result;
    }

    public IEnumerable<StatisticsStudentsProjects> GetStatisticsStudentsProjects(int? matchingId, int? studentId) //TODO DPL-42 переписать на EF
    {
        var model = statisticsRepositoryOld.GetStatisticsStudentsProjects(matchingId.Value, studentId.Value);
        return model.OrderBy(project => project.OrderNumber);
    }

    public IEnumerable<ProjectDto> GetStatisticsTutorsProjects(int? matchingId, int? tutorId) //TODO DPL-42 переписать на EF
    {
        var result = statisticsRepositoryOld.GetStatisticsTutorsProjects(matchingId.Value, tutorId.Value);
        return result;
    }
}