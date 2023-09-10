using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Role.service;
using MatchingSystem.DataLayer.Feature.User.service;

namespace MatchingSystem.Service.Notification;

public class NotificationService : INotificationService
{
    private readonly IExecutiveRepository executiveRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly IUserRepositoryOld userRepositoryOld;
    private readonly ICommonQuotaRepository commonQuotaRepository;

    public NotificationService(ITutorRepository tutorRepository, IExecutiveRepository executiveRepository, ICommonQuotaRepository commonQuotaRepository, IUserRepositoryOld userRepositoryOld)
    {
        this.tutorRepository = tutorRepository;
        this.executiveRepository = executiveRepository;
        this.commonQuotaRepository = commonQuotaRepository;
        this.userRepositoryOld = userRepositoryOld;
    }

    public void ReadNotifications(int userId, int matchingId)
    {
        userRepositoryOld.ReadNotifications(userId,matchingId, 0);
        //Нужно сделать транзакционно - TODO DPL-14 сделать задачкку на прочтение нотификаций
        // List<CommonQuota> quotas = commonQuotaRepository.find(quota => quota.IsNotification == true).ToList();//найти квоту соотвествующую этому юзеру в этом матчинге
        //foreach (var commonQuota in quotas)
        //{
        //    commonQuota.IsNotification = false;
        //    //commonQuotaRepository.update(commonQuota);
        //}
    }
    public void ReadNotificationsForTutor(int userId, int matchingId, int tutorId)
    {
        userRepositoryOld.ReadNotifications(userId,matchingId, tutorId);
 
    }
    public Dictionary<string, int> GetNotificationsByTutor(int tutorId, int userId, int matchingId)
    {
        var count = tutorRepository.GetNotificationsCountByTutor(tutorId);
        var result = new Dictionary<string, int> { { "count", count } };
        ReadNotificationsForTutor(userId, matchingId,tutorId );
        return result;
    }

    public Dictionary<string, int> GetNotificationsByExecutive(int userId, int matchingId)
    {
        var actionResult = new Dictionary<string, int>(1);
        var result = executiveRepository.GetNotificationsCountByExecutive(userId, matchingId);
        actionResult.Add("count", result);
        ReadNotifications(userId, matchingId);
        return actionResult;
    }
}