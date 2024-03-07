using System.Collections.Generic;

namespace MatchingSystem.Service.Statistics;

public class StageStatistics
{
    public static readonly List<StatisticsProcessorEnum> TUTOR_STAGE_STATISTICS = new()
    {
        StatisticsProcessorEnum.ALL_TUTORS_COUNT,
        StatisticsProcessorEnum.ALL_STUDENTS_COUNT,
        StatisticsProcessorEnum.TUTORS_VISITED_COUNT,
        StatisticsProcessorEnum.STUDENTS_VISITED_COUNT,
        StatisticsProcessorEnum.TUTORS_READY_COUNT,
        StatisticsProcessorEnum.QUOTA_SUM,
        StatisticsProcessorEnum.QUOTA_AVG,
        StatisticsProcessorEnum.ALL_PROJECTS_COUNT,
        StatisticsProcessorEnum.PROJECTS_PER_TUTOR_AVG,
    };

    public static readonly List<StatisticsProcessorEnum> STUDENTS_STAGE_STATISTICS = new()
    {
        StatisticsProcessorEnum.ALL_TUTORS_COUNT,
        StatisticsProcessorEnum.ALL_STUDENTS_COUNT,
        StatisticsProcessorEnum.TUTORS_VISITED_COUNT,
        StatisticsProcessorEnum.STUDENTS_VISITED_COUNT,
        StatisticsProcessorEnum.STUDENTS_READY_COUNT,
        StatisticsProcessorEnum.AVG_SELECTED_PROJECTS,
        StatisticsProcessorEnum.TOP_3_PROJECTS,
        StatisticsProcessorEnum.TOP_3_TUTORS,
    };
    
    public static readonly List<StatisticsProcessorEnum> ITERATION_STAGE_STATISTIC = new()
    {
        StatisticsProcessorEnum.ALL_TUTORS_COUNT,
        StatisticsProcessorEnum.ALL_STUDENTS_COUNT,
        StatisticsProcessorEnum.TUTORS_VISITED_COUNT,
        StatisticsProcessorEnum.STUDENTS_VISITED_COUNT,
        StatisticsProcessorEnum.STUDENTS_ALLOCATED,
        StatisticsProcessorEnum.STUDENTS_POSSIBLY_ALLOCATED,
        StatisticsProcessorEnum.STUDENTS_CANT_BE_ALLOCATED,
        StatisticsProcessorEnum.PROJECTS_CLOSED_COUNT,
    };
}