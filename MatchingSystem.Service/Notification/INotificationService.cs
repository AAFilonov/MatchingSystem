namespace Service.Notification;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;

public interface INotificationService
{
    /*
    void ReadNotifications(int userId, int matchingId);
    Dictionary<string, int> getNotificationsByExecutive(int userId, int matchingId);
    */
    public IActionResult GetNotificationsByTutor(int tutorId, int userId, int matchingId);
    public IActionResult GetNotificationsByExecutive(int userId, int matchingId);
}