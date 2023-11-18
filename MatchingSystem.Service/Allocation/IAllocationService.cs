using MatchingSystem.DataLayer.OldDto;

namespace MatchingSystem.Service.Allocation;

public interface IAllocationService
{
    AllocationData GetAllocations();
}