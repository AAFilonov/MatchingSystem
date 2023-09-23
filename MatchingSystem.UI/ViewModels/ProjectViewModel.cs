using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ViewModels
{
    public class ProjectViewModel
    {
        public int? tutorId { get; set; }
        public int CommonQuota { get; set; }
        public List<Project> Projects { get; set; }
        public List<Group> Groups { get; set; }
        public List<TechnologyDto> Technology { get; set; }
        public List<WorkDirectionDto> WorkDirections { get; set; }
        public int? IsReady { get; set; }
    }
}
