using System;
using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class StudentsReadyCountProcessors : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Students
            .Where(entity =>entity.MatchingId == matchingId )
            .Include(entity => entity.StudentsPreferences)
            .ToList();//TODO DPL-41 фильтр по isDeleted

        var ready = query.Where(entity => entity.StudentsPreferences.Any()).ToList();
        var percentage = Math.Round(ready.Count / (double) query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Сделали выбор: Студентов",
            StatValue = ready.Count,
            StatValue_Str = $"{ready.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}