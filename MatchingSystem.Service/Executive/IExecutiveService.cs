using System;
using MatchingSystem.DataLayer.Dto;

namespace MatchingSystem.Service.Executive;
public interface IExecutiveService
{
    public void SetNextStage(int? matchingId, int? userId);

    public void SetEndDate(DateTime endDate, int matchingId);

    public AdjustmentData GetAllocationByExecutive(int? userId, int? matchingId);

    public void SetAllocationByExecutive(AdjustmentRequest request);

}
