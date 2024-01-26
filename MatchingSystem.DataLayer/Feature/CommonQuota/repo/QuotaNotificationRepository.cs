using System.Collections.Generic;
using System.Linq;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public class QuotaNotificationRepository : GenericRepository<CommonQuotaEntity>, IQuotaNotificationRepository
{
    public List<CommonQuotaEntity> GetNotificationsByTutor(int tutorId)
    {
      return find(entity => entity.Tutor.TutorId == tutorId && entity.IsNotification == true).ToList();
    }
    
    public List<CommonQuotaEntity> GetNotificationsByExecutive( int matchingId)
    {
        return find(entity =>
                entity.Stage.MatchingId == matchingId &&
                entity.IsNotification == true &&
                entity.QuotaStateId == (int)CommonQuotaStateEnum.REQUESTED
            ).ToList();
    }
}