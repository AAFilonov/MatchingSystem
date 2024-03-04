using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.Service.Statistics.StatisticsProcessors;

namespace MatchingSystem.Service.Statistics;

public class MainStatisticsService : Repository, IMainStatisticsService
{
    private readonly IMatchingRepository matchingRepository;

    public MainStatisticsService(IMatchingRepository matchingRepository)
    {
        this.matchingRepository = matchingRepository;
    }

    Dictionary<StatisticsProcessorEnum, IStatisticProcessor> processors = new()
    {
        {StatisticsProcessorEnum.ALL_TUTORS_COUNT, new AllTutorsCountProcessor()},
        {StatisticsProcessorEnum.ALL_STUDENTS_COUNT, new AllStudentsCountProcessor()},
        {StatisticsProcessorEnum.TUTORS_VISITED_COUNT, new TutorsVisitedCountProcessor()},
        {StatisticsProcessorEnum.STUDENTS_VISITED_COUNT, new StudentsVisitedCountProcessor()},
        {StatisticsProcessorEnum.TUTORS_READY_COUNT, new TutorsReadyCountProcessors()},
        {StatisticsProcessorEnum.QUOTA_SUM, new QuotaSumProcessor()},
        {StatisticsProcessorEnum.QUOTA_AVG, new QuotaAvgProcessor()},
        {StatisticsProcessorEnum.ALL_PROJECTS_COUNT, new AllProjectsCountProcessor()},
        {StatisticsProcessorEnum.PROJECTS_PER_TUTOR_AVG, new ProjectsPerTutorAvg()},
        {StatisticsProcessorEnum.STUDENTS_READY_COUNT, new StudentsReadyCountProcessors()},
        {StatisticsProcessorEnum.AVG_SELECTED_PROJECTS, new AvgSelectedProjectsProcessor()},
        {StatisticsProcessorEnum.TOP_3_PROJECTS, new Top3ProjectsProcessor()},
        {StatisticsProcessorEnum.TOP_3_TUTORS, new Top3TutorsProcessor()},
        {StatisticsProcessorEnum.STUDENTS_ALLOCATED, new StudentsAllocatedProcessor()},
        {StatisticsProcessorEnum.STUDENTS_POSSIBLY_ALLOCATED, new StudentsPossiblyAllocatedProcessor()},
        {StatisticsProcessorEnum.STUDENTS_CANT_BE_ALLOCATED, new StudentsCantBeAllocatedProcessor()},
        {StatisticsProcessorEnum.PROJECTS_CLOSED_COUNT, new ProjectsClosedCountProcessor()},
    };
    
    public List<StatisticsMain> GetCurrentStageStatistics(int matchingId)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);
        return GetStatistics(matchingId, currentStage.StageTypeId);
    }

    public List<StatisticsMain> GetStatistics(int matchingId, int currentStageTypeId)
    {
        //StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);
        List<StatisticsMain> result = new List<StatisticsMain>();
        switch (currentStageTypeId)
        {
            case (int) StageTypeEnum.MATCHING_INITIALIZATION:
                result = GetStatistics(matchingId, StageStatistics.TUTOR_STAGE_STATISTICS);
                break;
            case (int) StageTypeEnum.MATCHING_PREPARATION:
                result = GetStatistics(matchingId, StageStatistics.TUTOR_STAGE_STATISTICS);
                break;
            case (int) StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES:
                result = GetStatistics(matchingId, StageStatistics.STUDENTS_STAGE_STATISTICS);
                break;
            case (int) StageTypeEnum.ITERATION:
                result = GetStatistics(matchingId, StageStatistics.ITERATION_STAGE_STATISTIC);
                break;
            case (int) StageTypeEnum.MANUAL_ADJUSTMENT:
                result = GetStatistics(matchingId, StageStatistics.ITERATION_STAGE_STATISTIC);
                break;
            case (int) StageTypeEnum.FINAL:
                result = GetStatistics(matchingId, StageStatistics.ITERATION_STAGE_STATISTIC);
                break;
        }

        return result;
    }

    private List<StatisticsMain> GetStatistics(int matchingId, List<StatisticsProcessorEnum> statistics)
    {
        return CallInContext(context =>
        {
            var result = new List<StatisticsMain>();
            foreach (var statisticName in statistics)
            {
                result.Add(processors[statisticName].process(context, matchingId));
            }
            return result;
        });
    }
}