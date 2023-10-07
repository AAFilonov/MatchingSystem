using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class TutorQuotaData
    {
        public List<QuotaHistoryTutor> History { get; set; }
        public List<ProjectDto> Projects { get; set; }
        public int? CommonQuota { get; set; }
    }
}