using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public interface IQuotaRequestRepository : IRepository<CommonQuotaEntity>
{
    public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId, int matchingId);
    IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int dataSelectedMatching);
    IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId);
    public CommonQuotaEntity? GetActiveTutorQuota(int tutorId, int matchingId);
    
}