using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Interface
{
    public interface IMatchingRepository
    {
        Task SetStageEndDateAsync(DateTime date, int matchingId);
        void SetStageEndDate(DateTime date, int matchingId);
        Task SetNextStageAsync(int matchingId);
        void SetNextStage(int matchingId);
        int CreateMatching(MatchingInitDto data);
        int SetNewFirstStageInMatching(int MatchingID);
        Task<IEnumerable<MatchingInfo>> GetMatchingsInfoAsync();
        IEnumerable<MatchingInfo> GetMatchingsInfo();
        Task<IEnumerable<Allocation>> GetFinalAllocationsAsync();
        IEnumerable<Allocation> GetFinalAllocations();
        public IEnumerable<Allocation> GetFinalAllocationByMatching(int MatchingId);
        Task<IEnumerable<OldEntities.Matching>> GetMatchingsByUserAsync(int userId);
        IEnumerable<OldEntities.Matching> GetMatchingsByUser(int userId);
        Task<Stage> GetCurrentStageAsync(int matchingId);
        Stage GetCurrentStage(int matchingId);
        IEnumerable<OldEntities.Matching> GetMatchings();
        Task<IEnumerable<OldEntities.Matching>> GetMatchingsAsync();
    }
}
