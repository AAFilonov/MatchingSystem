using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IExecutiveRepository
    {
        Task SetAdjustmentByExecutiveAsync(AdjustmentParams @params);
        void SetAdjustmentByExecutive(AdjustmentParams @params);
        Task<IEnumerable<Allocation>> GetAllocationsByExecutiveAsync(int userId, int matchingId);
        IEnumerable<Allocation> GetAllocationsByExecutive(int userId, int matchingId);
        Task<int> GetNotificationsCountByExecutiveAsync(int userId, int matchingId);

        int GetNotificationsCountByExecutive(int userId, int matchingId);

        //prev: GetQuotaRequestAsync
        Task<IEnumerable<QuotaRequest>> GetQuotaRequestsByExecutiveAsync(int userId, int matchingId);

        IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId);

        //prev: GetCommonQuotaHistoryByExecutiveAsync
        Task<IEnumerable<QuotaHistoryExecutive>> GetQuotaRequestHistoryByExecutiveAsync(int userId, int matchingId);

        IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int userId, int matchingId);

        Task AcceptQuotaRequestAsync(int quotaId, bool result);
        void AcceptQuotaRequest(int quotaId, bool result);
       
   
    }
}
