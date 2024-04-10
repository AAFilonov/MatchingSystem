using System;
using MatchingSystem.DataLayer.OldDto;

namespace MatchingSystem.Service.Executive;

public interface IExecutiveService
{
    public void SetNextStage(int? matchingId, int? userId);

    public void SetEndDate(DateTime endDate, int matchingId);

    public AdjustmentData GetAllocationByExecutive(int userId, int matchingId);

}