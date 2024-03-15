using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace OWAdministrativeService.Models;

public class Form : IEntity
{
    [BsonId] public Guid Id { get; set; } = Guid.Empty;
    public string Email { get; set; }
    public string Body { get; set; }
    public string FileSrc { get; set; }
    public string FileName { get; set; }
    public FormType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public FormStatus Status { get; set; } = FormStatus.Pending;
    public DateTime? DateToGetResult { get; set; }
    public string Note { get; set; }

    public object GenerateNewID()
    {
        return Guid.NewGuid();
    }

    public bool HasDefaultID()
    {
        return Id == Guid.Empty;
    }
}

public enum FormStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}