using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class TutorMonitoringDto
{
    public int tutorId { get; set; }
    public string nameAbbreviation { get; set; }
    public string quota { get; set; }
    public List<ProjectMonitoringDto> projects { get; set; }
    public List<StudentMonitoringDto> waitingList { get; set; }

    public override string ToString()
    {
        return $"[{nameof(tutorId)}: {tutorId}, {nameof(nameAbbreviation)}: {nameAbbreviation}, {nameof(quota)}: {quota}]";
    }
}