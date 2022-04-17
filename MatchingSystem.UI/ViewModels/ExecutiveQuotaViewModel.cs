using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ViewModels
{
    public class ExecutiveQuotaViewModel
    {
        public IEnumerable<QuotaRequest> Requests { get; set; }
        public IEnumerable<QuotaHistoryExecutive> History { get; set; }
    }
}
