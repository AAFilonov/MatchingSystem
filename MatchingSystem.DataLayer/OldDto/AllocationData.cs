using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class AllocationData
{
    public IEnumerable<AllocationDto> Allocations { get; set; }
    public IEnumerable<MatchingDto> Matchings { get; set; }
}