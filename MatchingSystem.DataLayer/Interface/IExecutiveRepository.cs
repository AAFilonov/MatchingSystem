using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Request;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IExecutiveRepository
    {
        Task SetAdjustmentByExecutiveAsync(AdjustmentRequest request);
        Task<IEnumerable<Allocation>> GetAllocationsByExecutiveAsync(int userId, int matchingId);
        Task<int> GetNotificationsCountByExecutive(int userId, int matchingId);
        //prev: GetQuotaRequestAsync
        Task<IEnumerable<QuotaRequest>> GetQuotaRequestsByExecutiveAsync(int userId, int matchingId);
        //prev: GetCommonQuotaHistoryByExecutiveAsync
        Task<IEnumerable<QuotaHistoryExecutive>> GetQuotaRequestHistoryByExecutiveAsync(int userId, int matchingId);
        //prev: GetMainStatisticsAsync
        Task<IEnumerable<MainStatistics>> GetStatistics(int matchingId, int currentStage);
    }
}
