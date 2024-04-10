using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Stage;

namespace MatchingSystem.DataLayer.Feature.Matching.repo;

public class MatchingRepository : GenericRepository<MatchingEntity>, IMatchingRepository
{
    public List<AllocationFullInfoView> GetAllocationsFullInfoViews()
    {
        List<AllocationFullInfoView> allocations = _matchingSystemContext.AllocationFullInfos.ToList();
        return allocations;
    }
    
    public StageEntity GetCurrentStage(int matchingId)
    {
        return findById(matchingId).Stages.Single(entity => entity.IsCurrent == true);
    }
}