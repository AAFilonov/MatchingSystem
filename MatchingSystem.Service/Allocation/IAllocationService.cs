using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Dto;

namespace Service.Allocation;

public interface IAllocationService
{
    void ReadNotifications(int userId, int matchingId);
    AllocationData GetAllocations();
}
