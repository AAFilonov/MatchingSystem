using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ViewModels
{
    public class ExecutiveQuotaViewModel
    {
        public IEnumerable<QuotaRequest> Requests { get; set; }
        public IEnumerable<QuotaHistoryExecutive> History { get; set; }
    }
}
