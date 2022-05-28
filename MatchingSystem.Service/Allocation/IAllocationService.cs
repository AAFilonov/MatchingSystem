using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;

namespace Service.Allocation;

public interface IAllocationService
{
    AllocationData GetAllocations();
}
