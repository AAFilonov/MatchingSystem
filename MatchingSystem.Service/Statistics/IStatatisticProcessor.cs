using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics;

public interface IStatisticProcessor
{
    public StatisticsMain process(DiplomaMatchingContext context, int matchingId);
}