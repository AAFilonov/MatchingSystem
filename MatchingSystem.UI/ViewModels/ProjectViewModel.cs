using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ViewModels
{
    public class ProjectViewModel
    {
        public int? tutorId { get; set; }
        public int CommonQuota { get; set; }
        public List<ProjectDto> Projects { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<TechnologyDto> Technology { get; set; }
        public List<WorkDirectionDto> WorkDirections { get; set; }
        public int? IsReady { get; set; }
    }
}
