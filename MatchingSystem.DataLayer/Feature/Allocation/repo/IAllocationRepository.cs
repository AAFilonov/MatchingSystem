using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto;

namespace MatchingSystem.DataLayer.Feature.Allocation.repo;

public interface IAllocationRepository
{
    public IEnumerable<AllocationDto> GetAllocationsByExecutive(int userId, int matchingId);
    public void SetAdjustmentByExecutive(AdjustmentRequest inParams);

}