using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Matching.service
{
    public interface IMatchingRepository: IRepository<Matching>
    {
        IEnumerable<Allocation> GetFinalAllocations();
        StageDto GetCurrentStageDto(int matchingId);
        Stage.Stage GetCurrentStage(int matchingId);
        IEnumerable<MatchingDto> GetMatchingsDto();
        IEnumerable<Matching> GetMatchings();
    }
}
