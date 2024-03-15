using Microsoft.EntityFrameworkCore;

namespace StudentService.Models;

[Index(nameof(CreatedAt))]
[Index(nameof(UpdatedAt))]
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}