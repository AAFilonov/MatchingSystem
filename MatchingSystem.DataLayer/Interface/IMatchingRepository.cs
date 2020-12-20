using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Interface
{
    interface IMatchingRepository
    {
        Task SetStageEndDateAsync(DateTime date, int matchingId);
        Task SetNextStageAsync(int matchingId);
        Task<IEnumerable<MatchingInfo>> GetMatchingsInfoAsync();
        Task<IEnumerable<Allocation>> GetFinalAllocationsAsync();
        Task<IEnumerable<Matching>> GetMatchingsByUserAsync(int userId);
        Task<Stage> GetCurrentStageAsync(int matchingId);

    }
}
