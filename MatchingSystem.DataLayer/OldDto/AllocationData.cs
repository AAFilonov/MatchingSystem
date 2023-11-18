using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Matching;

namespace MatchingSystem.DataLayer.OldDto;
public class AllocationData
{
    public IEnumerable<AllocationDto> Allocations { get; set; }
    public IEnumerable<MatchingDto> Matchings { get; set; }
}