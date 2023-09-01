using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.User.service;

namespace MatchingSystem.Service.Notification;

public class NotificationService : INotificationService
{
    private readonly IExecutiveRepository executiveRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly IUserRepository userRepository;

    public NotificationService(ITutorRepository tutorRepository, IExecutiveRepository executiveRepository, IUserRepository userRepository)
    {
        this.tutorRepository = tutorRepository;
        this.executiveRepository = executiveRepository;
        this.userRepository = userRepository;
    }

    public void ReadNotifications(int userId, int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, int> GetNotificationsByTutor(int tutorId, int userId, int matchingId)
    {
        var count = tutorRepository.GetNotificationsCountByTutor(tutorId);
        var result = new Dictionary<string, int> { { "count", count } };
        ReadNotifications(userId, matchingId);
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