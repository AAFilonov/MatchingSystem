using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public interface IQuotaNotificationRepository : IRepository<CommonQuotaEntity>
{
    List<CommonQuotaEntity> GetNotificationsByTutor(int tutorId);
    List<CommonQuotaEntity> GetNotificationsByExecutive(int matchingId);
}