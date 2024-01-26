using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;

namespace MatchingSystem.Service.Notification;

public class QuotaNotificationService : IQuotaNotificationService
{
    private readonly IQuotaNotificationRepository quotaNotificationRepository;

    public QuotaNotificationService(IQuotaNotificationRepository quotaNotificationRepository)
    {
        this.quotaNotificationRepository = quotaNotificationRepository;
    }

    public Dictionary<string, int> ReadNotificationsByTutor(int tutorId)
    {
        List<CommonQuotaEntity> quotaEntities = quotaNotificationRepository.GetNotificationsByTutor(tutorId);
        ReadNotifications(quotaEntities);
        var result = new Dictionary<string, int> { { "count", quotaEntities.Count } };
        return result;
    }

    public Dictionary<string, int> ReadNotificationsByExecutive(int matchingId)
    {
        var quotaEntities = quotaNotificationRepository.GetNotificationsByExecutive(matchingId);
        ReadNotifications(quotaEntities);
        var actionResult = new Dictionary<string, int>(1) { { "count", quotaEntities.Count } };
        return actionResult;
    }

    private void ReadNotifications(List<CommonQuotaEntity> quotaEntities)
    {
        quotaEntities.ForEach(entity => entity.IsNotification = false);
        quotaNotificationRepository.updateRange(quotaEntities);
    }
}