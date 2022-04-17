using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ViewModels
{
    public class ProjectViewModel
    {
        public int? tutorId { get; set; }
        public int CommonQuota { get; set; }
        public List<Project> Projects { get; set; }
        public List<Group> Groups { get; set; }
        public List<Technology> Technology { get; set; }
        public List<WorkDirection> WorkDirections { get; set; }
        public int? IsReady { get; set; }
    }
}
