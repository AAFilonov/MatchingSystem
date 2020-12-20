using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.ResultModels
{
    public class AllocationData
    {
        public List<Allocation> Allocations { get; set; }
        public List<MatchingInfo> Matchings { get; set; }
    }
}