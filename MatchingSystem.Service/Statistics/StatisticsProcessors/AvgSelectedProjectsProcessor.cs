using System;
using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class AvgSelectedProjectsProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Students
            .Where(entity => entity.MatchingId == matchingId)
            .Include(entity => entity.StudentsPreferences)
            .ToList();
        
        double value = Math.Round(
            query.Average(entity => entity.StudentsPreferences.Count),
            2
        );
        return new StatisticsMain
        {
            StatName = "Среднее количество выбранных проектов",
            StatValue = value,
            StatValue_Str = value.ToString(),
        };
    }
}