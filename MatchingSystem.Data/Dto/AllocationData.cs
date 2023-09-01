using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class AllocationData
{
    public IEnumerable<Allocation> Allocations { get; set; }
    public IEnumerable<MatchingInfo> Matchings { get; set; }
}