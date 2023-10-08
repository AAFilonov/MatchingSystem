using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.Service.Monitoring;

public interface IMonitoringService
{
    MatchingMonitoringData getMonitoringData(int matchingId);
    List<StudentMonitoringDto> getStudentsMonitoringData(int matchingId);
    List<TutorMonitoringDto> getTutorsMonitoringData(int matchingId);
}