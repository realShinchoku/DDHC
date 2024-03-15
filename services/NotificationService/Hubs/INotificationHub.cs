using NotificationService.Models;

namespace NotificationService.Hubs;

public interface INotificationHub
{
    public Task ReceiveNotification(Notification notification);
    public Task ReceiveNotifications(ICollection<Notification> notifications);
}