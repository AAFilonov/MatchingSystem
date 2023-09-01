using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class AllocationData
    {
        public List<Allocation> Allocations { get; set; }
        public List<MatchingInfo> Matchings { get; set; }
    }
}