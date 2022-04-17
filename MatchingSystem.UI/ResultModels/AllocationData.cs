using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ResultModels
{
    public class AllocationData
    {
        public IEnumerable<Allocation> Allocations { get; set; }
        public IEnumerable<MatchingInfo> Matchings { get; set; }
    }
}