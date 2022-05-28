using MatchingSystem.Data.Feature.User;

namespace Service.Notification;
using MatchingSystem.DataLayer.Interface;


public class NotificationService  : INotificationService
{
    private readonly ITutorRepository tutorRepository;
    private readonly IExecutiveRepository executiveRepository;
    private readonly IUserRepository userRepository;

    public NotificationService(ITutorRepository tutorRepository, IExecutiveRepository executiveRepository, IUserRepository userRepository)
    {
        this.tutorRepository = tutorRepository;
        this.executiveRepository = executiveRepository;
        this.userRepository = userRepository;
    }

    /*
    public void ReadNotifications(int userId, int matchingId)
    {
        throw new NotImplementedException();
    }*/

    public Dictionary<string, int> GetNotificationsByTutor(int tutorId, int userId, int matchingId)
    {
        var count = tutorRepository.GetNotificationsCountByTutor(tutorId);

        var result = new Dictionary<string, int> { { "count", count } };

        userRepository.ReadNotifications(userId, matchingId, tutorId);

        return result;
    }

    public Dictionary<string, int> GetNotificationsByExecutive(int userId, int matchingId)
    {
        var actionResult = new Dictionary<string, int>(1);

        var result = executiveRepository.GetNotificationsCountByExecutive(userId, matchingId);

        actionResult.Add("count", result);
        userRepository.ReadNotifications(userId, matchingId);

        return actionResult;
    }


    /*public  Dictionary<string, int> getNotificationsByExecutive(int userId, int matchingId)
    {
        throw new NotImplementedException();
        
    }*/
}