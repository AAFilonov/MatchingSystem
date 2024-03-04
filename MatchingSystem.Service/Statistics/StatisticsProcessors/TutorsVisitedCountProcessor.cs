using System;
using System.Linq;
using Dapper;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class TutorsVisitedCountProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.TutorsViews
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();
        var currentStage = context.Stages.Single(entity => entity.MatchingId == matchingId && entity.IsCurrent == true);
        var visited = query.AsList().Where(view => view?.LastVisitDate > currentStage.StartDate).ToList();
        var percentage = Math.Round(visited.Count / (double) query.Count, 2);
        return new StatisticsMain
        {
            StatName = "Посетили систему на текущем этапе: Преподавателей",
            StatValue = visited.Count,
            StatValue_Str = $"{visited.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}