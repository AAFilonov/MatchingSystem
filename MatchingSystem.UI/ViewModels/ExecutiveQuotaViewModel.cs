using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ViewModels
{
    public class ExecutiveQuotaViewModel
    {
        public List<QuotaRequest> Requests { get; set; }
        public List<QuotaHistoryExecutive> History { get; set; }
    }
}
