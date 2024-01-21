using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;

namespace MatchingSystem.Service.Notification;

public class QuotaQuotaNotificationService : IQuotaNotificationService
{
    private readonly ICommonQuotaRepository commonQuotaRepository;

    public QuotaQuotaNotificationService(ICommonQuotaRepository commonQuotaRepository)
    {
        this.commonQuotaRepository = commonQuotaRepository;
    }
    
    public Dictionary<string, int> ReadNotificationsByTutor(int tutorId)
    {
        List<CommonQuotaEntity> quotaEntities = commonQuotaRepository.GetNotificationsByTutor(tutorId);
        quotaEntities.ForEach(entity =>entity.IsNotification=false );
        commonQuotaRepository.updateRange(quotaEntities);
        
        var result = new Dictionary<string, int> { { "count", quotaEntities.Count } };
        return result;
    }

    public Dictionary<string, int> ReadNotificationsByExecutive(int matchingId)
    {
        List<CommonQuotaEntity> quotaEntities = commonQuotaRepository.GetNotificationsByExecutive(matchingId);
        quotaEntities.ForEach(entity =>entity.IsNotification=false );
        commonQuotaRepository.updateRange(quotaEntities);
        
        var actionResult = new Dictionary<string, int>(1);
        actionResult.Add("count", quotaEntities.Count);
        return actionResult;
    }
}