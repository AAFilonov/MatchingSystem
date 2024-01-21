using System.Collections.Generic;

namespace MatchingSystem.Service.Notification;

public interface IQuotaNotificationService
{
    public Dictionary<string, int> ReadNotificationsByTutor(int tutorId);
    public Dictionary<string, int> ReadNotificationsByExecutive(int matchingId);
}