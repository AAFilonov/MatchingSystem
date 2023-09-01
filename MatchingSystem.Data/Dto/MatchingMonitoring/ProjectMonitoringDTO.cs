namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class ProjectMonitoringDto
{
    public int projectId { get; set; }
    public string projectName { get; set; }
    public short? quota { get; set; }
    public string info { get; set; }
    public string availableGroupsNameList { get; set; }
    public string technologiesNameList { get; set; }
    public string workDirectionsNameList { get; set; }
    public int tutorId { get; set; }
    public int assignmentStudentId { get; set; }
    //for student
    public int? orderInStudentPrefs { get; set; }
    public bool? isActive { get; set; }
}