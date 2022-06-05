using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IMatchingRepository
    {
        Task SetStageEndDateAsync(DateTime date, int matchingId);
        void SetStageEndDate(DateTime date, int matchingId);
        Task SetNextStageAsync(int matchingId);
        void SetNextStage(int matchingId);
        Task<IEnumerable<MatchingInfo>> GetMatchingsInfoAsync();
        IEnumerable<MatchingInfo> GetMatchingsInfo();
        Task<IEnumerable<Allocation>> GetFinalAllocationsAsync();
        IEnumerable<Allocation> GetFinalAllocations();
        public IEnumerable<Allocation> GetFinalAllocationByMatching(int MatchingId);
        Task<IEnumerable<Matching>> GetMatchingsByUserAsync(int userId);
        IEnumerable<Matching> GetMatchingsByUser(int userId);
        Task<Stage> GetCurrentStageAsync(int matchingId);
        Stage GetCurrentStage(int matchingId);
        IEnumerable<Matching> GetMatchings();
        Task<IEnumerable<Matching>> GetMatchingsAsync();
    }
}
