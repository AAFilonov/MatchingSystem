using System.Linq;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics.StatisticsProcessors;

public class AllProjectsCountProcessor : IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId)
    {
        var query = context.Projects
            .Where(tutorView => tutorView.MatchingId == matchingId)
            .ToList();

        return new StatisticsMain()
        {
            StatName = "Проектов: всего",
            StatValue = query.Count(),
            StatValue_Str = query.Count().ToString()
        };
    }
}