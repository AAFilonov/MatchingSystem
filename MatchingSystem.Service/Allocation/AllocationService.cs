using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Feature.Interface;

namespace MatchingSystem.Service.Allocation;

public class AllocationService : IAllocationService
{
    private readonly IMatchingRepository matchingRepository;

    public AllocationService(IMatchingRepository matchingRepository)
    {
        this.matchingRepository = matchingRepository;
    }

    public AllocationData GetAllocations()
    {
        var model = new AllocationData
        {
            Allocations = matchingRepository.GetFinalAllocations(),
            Matchings = matchingRepository.GetMatchingsInfo()
        };
        return model;
    }
}