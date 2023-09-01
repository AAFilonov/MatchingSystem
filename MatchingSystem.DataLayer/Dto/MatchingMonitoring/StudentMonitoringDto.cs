using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class StudentMonitoringDto
{
    public string groupName { get; set; }
    public string nameAbbreviation { get; set; }
    public List<ProjectMonitoringDto> preferences { get; set; }
    public ProjectMonitoringDto? assignedProject  { get; set; }
    public int? orderInTutorPrefs { get; set; }

    public override string ToString()
    {
        return $"[{nameof(groupName)}: {groupName}, {nameof(nameAbbreviation)}: {nameAbbreviation}, {nameof(preferences)}: {preferences}, {nameof(assignedProject)}: {assignedProject}, {nameof(orderInTutorPrefs)}: {orderInTutorPrefs}]";
    }
}