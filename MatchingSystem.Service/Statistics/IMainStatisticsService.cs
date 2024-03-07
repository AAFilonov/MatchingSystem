using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Statistics;

public interface IMainStatisticsService
{
    public List<StatisticsMain> GetCurrentStageStatistics(int matchingId);
    public List<StatisticsMain> GetStatistics(int matchingId, int currentStageTypeId);
}