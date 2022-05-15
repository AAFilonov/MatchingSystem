namespace Service.Notification;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.UI.RequestModels;

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

    public IActionResult GetNotificationsByTutor(int tutorId, int userId, int matchingId)
    {
        var count = tutorRepository.GetNotificationsCountByTutor(tutorId);

        var result = new Dictionary<string, int> { { "count", count } };

        userRepository.ReadNotifications(userId, matchingId, tutorId);

        return new JsonResult(result);
    }

    public IActionResult GetNotificationsByExecutive(int userId, int matchingId)
    {
        var actionResult = new Dictionary<string, int>(1);

        var result = executiveRepository.GetNotificationsCountByExecutive(userId, matchingId);

        actionResult.Add("count", result);
        userRepository.ReadNotifications(userId, matchingId);

        return new JsonResult(actionResult);
    }


    /*public  Dictionary<string, int> getNotificationsByExecutive(int userId, int matchingId)
    {
        throw new NotImplementedException();
        
    }*/
}