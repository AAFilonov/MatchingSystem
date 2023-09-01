using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class AllocationData
{
    public IEnumerable<Allocation> Allocations { get; set; }
    public IEnumerable<MatchingDto> Matchings { get; set; }
}