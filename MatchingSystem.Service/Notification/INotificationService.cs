namespace Service.Notification;


public interface INotificationService
{
    /*
    void ReadNotifications(int userId, int matchingId);
    Dictionary<string, int> getNotificationsByExecutive(int userId, int matchingId);
    */
    public Dictionary<string, int> GetNotificationsByTutor(int tutorId, int userId, int matchingId);
    public Dictionary<string, int> GetNotificationsByExecutive(int userId, int matchingId);
}