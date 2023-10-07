using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;

public class TutorProjectsModel
{
    public int? CommonQuota { get; set; }
    public List<ProjectDto> Projects { get; set; }
    public IEnumerable<GroupDto> Groups { get; set; }
    public IEnumerable<TechnologyDto> Technology { get; set; }
    public IEnumerable<WorkDirectionDto> WorkDirections { get; set; }
    public bool? IsReady { get; set; }
}
