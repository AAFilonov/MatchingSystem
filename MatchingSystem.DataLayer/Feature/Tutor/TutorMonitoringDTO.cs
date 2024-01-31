using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto.MatchingMonitoring;

namespace MatchingSystem.DataLayer.Feature.Tutor;

public class TutorMonitoringDto
{
    public int tutorId { get; set; }
    public string nameAbbreviation { get; set; }
    public int? quota { get; set; }
    public List<int> projects { get; set; }
    public List<int> waitingList { get; set; }

    public override string ToString()
    {
        return $"[{nameof(tutorId)}: {tutorId}, {nameof(nameAbbreviation)}: {nameAbbreviation}, {nameof(quota)}: {quota}]";
    }
}