using System.Collections.Generic;

namespace MatchingSystem.DataLayer.OldDto.MatchingMonitoring;

public class StudentMonitoringDto
{
    public int studentId { get; set; }
    public string groupName { get; set; }
    public string nameAbbreviation { get; set; }
    public List<int> preferenceProjectIds { get; set; }
    public List<int> alreadyTryedProjectIds { get; set; }
    public int? assignedProjectId  { get; set; }
    public int? orderInTutorPrefs { get; set; }
}