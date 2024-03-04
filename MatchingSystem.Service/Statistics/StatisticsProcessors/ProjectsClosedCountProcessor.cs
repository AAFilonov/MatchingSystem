using System;
using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class ProjectsClosedCountProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Projects
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();
        var closed = query.Where(entity => entity.IsClosed).ToList();

        var percentage = Math.Round(closed.Count / (double) query.Count, 2);
        return new StatisticsMain()
        {
            StatName = "Проектов закрыто",
            StatValue = closed.Count(),
            StatValue_Str = $"{closed.Count} / {query.Count}",
            StatPercentage = percentage
        };
    }
}