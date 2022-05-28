using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;

namespace Service.Allocation;

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

    public void ReadNotifications(int userId, int matchingId)
    {
        throw new NotImplementedException();
    }
}