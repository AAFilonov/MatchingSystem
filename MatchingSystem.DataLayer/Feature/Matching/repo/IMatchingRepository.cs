using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Stage;

namespace MatchingSystem.DataLayer.Feature.Matching.repo
{
    public interface IMatchingRepository: IRepository<MatchingEntity>
    {
        IEnumerable<AllocationDto> GetFinalAllocations();
        StageDto GetCurrentStageDto(int matchingId);
        Stage.StageEntity GetCurrentStage(int matchingId);
        IEnumerable<MatchingDto> GetMatchingsDto();
    }
}
