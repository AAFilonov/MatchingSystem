using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ResultModels
{
    public class TutorQuotaData
    {
        public IEnumerable<QuotaHistoryTutor> History { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public int? CommonQuota { get; set; }
    }
}