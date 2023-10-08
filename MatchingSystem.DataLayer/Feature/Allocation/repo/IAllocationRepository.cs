using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;

namespace MatchingSystem.DataLayer.Feature.Allocation.repo;

public interface IAllocationRepository
{
    public IEnumerable<AllocationDto> GetAllocationsByExecutive(int userId, int matchingId);
    public void SetAdjustmentByExecutive(AdjustmentRequest inParams);

}