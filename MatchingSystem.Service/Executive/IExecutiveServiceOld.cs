using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Executive;

public interface IExecutiveServiceOld
{
    void SetAdjustmentByExecutive(AdjustmentParams @params);

    IEnumerable<AllocationView> GetAllocationsByExecutive(int userId, int matchingId);

    int GetNotificationsCountByExecutive(int userId, int matchingId);


    IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId);


    IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int userId, int matchingId);


    void AcceptQuotaRequest(int quotaId, bool result);

}