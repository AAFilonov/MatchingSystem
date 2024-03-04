using System;
using System.Linq;
using Dapper;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class TutorsReadyCountProcessors : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.TutorsViews
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();
        var ready = query.AsList()
            .Where(view => view.IsReadyToStart == true).ToList();
        var percentage = Math.Round(ready.Count / (double) query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Готово к распределению: Преподавателей",
            StatValue = ready.Count,
            StatValue_Str = $"{ready.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}