using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.MatchingMonitoring;

namespace MatchingSystem.Service.Monitoring;

public interface IMonitoringService
{
    MatchingMonitoringData getMonitoringData(int matchingId);
    List<StudentMonitoringDto> getStudentsMonitoringData(int matchingId);
    List<TutorMonitoringDto> getTutorsMonitoringData(int matchingId);
}