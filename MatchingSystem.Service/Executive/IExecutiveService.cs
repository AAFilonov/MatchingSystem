using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;


namespace Service.Executive;
public interface IExecutiveService
{
    public void SetNextStage(int? matchingId, int? userId);

    public void SetEndDate(string endDate, int matchingId);

    public AdjustmentData GetAllocationByExecutive(int? userId, int? matchingId);

    public void SetAllocationByExecutive(AdjustmentRequest request);

}
