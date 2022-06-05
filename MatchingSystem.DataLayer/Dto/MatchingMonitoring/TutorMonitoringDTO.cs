using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class TutorMonitoringDto
{
    public int tutorId { get; set; }
    public string nameAbbreviation { get; set; }
    public string quota { get; set; }
    private List<ProjectMonitoringDto> projects { get; set; }
    private List<StudentMonitoringDto> waitingList { get; set; }
}