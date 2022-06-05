using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class MatchingMonitoringData
{
    public List<StudentMonitoringDto> studentRecords { get; set; } = new();
    public List<TutorMonitoringDto> tutorRecords { get; set; } = new();
}