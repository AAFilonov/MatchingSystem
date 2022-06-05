using MatchingSystem.DataLayer.Dto.MatchingMonitoring;

namespace MatchingSystem.Service.Monitoring;

public interface IMonitoringService
{
    MatchingMonitoringData getMonitoringData(int matchingId);
}
