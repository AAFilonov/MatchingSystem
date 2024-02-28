using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Stage;

namespace MatchingSystem.DataLayer.Feature.Matching.repo
{
    public interface IMatchingRepository: IRepository<MatchingEntity>
    {
        List<AllocationFullInfoView> GetAllocationsFullInfoViews();
        StageEntity GetCurrentStage(int matchingId);
    }
}
