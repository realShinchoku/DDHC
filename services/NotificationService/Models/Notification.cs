using Contracts.Notifications;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace NotificationService.Models;

public class Notification : IEntity
{
    [BsonId] public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }

    public object GenerateNewID()
    {
        return Guid.NewGuid();
    }

    public bool HasDefaultID()
    {
        return Id == Guid.Empty;
    }
}