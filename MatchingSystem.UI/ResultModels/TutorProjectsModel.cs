using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Project;

namespace MatchingSystem.UI.ResultModels
{
    public class TutorProjectsModel
    {
        public int CommonQuota { get; set; }
        public List<ProjectDto> Projects { get; set; }
        public IEnumerable<GroupDto> Groups { get; set; }
        public IEnumerable<TechnologyDto> Technology { get; set; }
        public IEnumerable<WorkDirectionDto> WorkDirections { get; set; }
        public int? IsReady { get; set; }
    }
}
