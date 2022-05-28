using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Dto;

public class TutorProjectsModel
{
    public int CommonQuota { get; set; }
    public List<Project> Projects { get; set; }
    public IEnumerable<Group> Groups { get; set; }
    public IEnumerable<Technology> Technology { get; set; }
    public IEnumerable<WorkDirection> WorkDirections { get; set; }
    public bool IsReady { get; set; }
}
