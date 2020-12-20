using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ResultModels
{
    public class TutorQuotaData
    {
        public List<QuotaHistoryTutor> History { get; set; }
        public List<Project> Projects { get; set; }
        public int? CommonQuota { get; set; }
    }
}