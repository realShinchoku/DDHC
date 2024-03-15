using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace AttendanceService.Models;

public class BaseEntity : IEntity
{
    [BsonId] public Guid Id { get; set; } = Guid.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public object GenerateNewID()
    {
        return Guid.NewGuid();
    }

    public bool HasDefaultID()
    {
        return Id == Guid.Empty;
    }
}