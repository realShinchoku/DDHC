namespace Contracts.Notifications;

public class NotificationCreated
{
    public string Title { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
}