using System.Collections.Generic;

namespace MatchingSystem.DataLayer.OldDto.MatchingMonitoring;

public class ProjectMonitoringDto
{
    public int projectId { get; set; }
    public string projectName { get; set; }
    public string quota { get; set; }
    public string info { get; set; }
    public string availableGroupsNameList { get; set; }
    public string technologiesNameList { get; set; }
    public string workDirectionsNameList { get; set; }
    public int tutorId { get; set; }
    public List<int> assignedStudentsIds { get; set; }
    public List<int> assignedStudentsIdsInQuota { get; set; }
}