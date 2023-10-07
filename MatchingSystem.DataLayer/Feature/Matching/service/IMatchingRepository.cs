using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Matching.service
{
    public interface IMatchingRepository: IRepository<MatchingEntity>
    {
        IEnumerable<Allocation> GetFinalAllocations();
        StageDto GetCurrentStageDto(int matchingId);
        Stage.StageEntity GetCurrentStage(int matchingId);
        IEnumerable<MatchingDto> GetMatchingsDto();
        IEnumerable<MatchingEntity> GetMatchings();
    }
}
