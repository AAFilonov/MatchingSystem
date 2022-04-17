namespace Service.Notification;

public interface INotificationService
{
    void ReadNotifications(int userId, int matchingId);
    Dictionary<string, int> getNotificationsByExecutive(int userId, int matchingId);

}