using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public interface ICommonQuotaRepository : IRepository<CommonQuotaEntity>
{
    public void CreateCommonQuotas(List<TutorInitDto> tuts, int stageId, DiplomaMatchingContext? extContext = null);
    public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId, int matchingId);
    IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int dataSelectedMatching);
    public CommonQuotaEntity? GetActiveTutorQuota(int tutorId, int matchingId);
    IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId);
    List<CommonQuotaEntity> GetNotificationsByTutor(int tutorId);
    List<CommonQuotaEntity> GetNotificationsByExecutive(int matchingId);
}