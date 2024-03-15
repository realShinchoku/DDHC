using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentService.Models;

[Index(nameof(Room))]
[Index(nameof(Code))]
[Index(nameof(DateStart))]
[Index(nameof(DateEnd))]
public class Subject : BaseEntity
{
    public Guid TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))] public virtual Teacher Teacher { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; }
    [MaxLength(10)] public string Room { get; set; }
    [MaxLength(50)] public string Name { get; set; }
    [MaxLength(10)] public string Code { get; set; }
    public bool IsEnded => DateEnd < DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly DateStart { get; set; }
    public DateOnly DateEnd { get; set; }

    public virtual ICollection<Student> Students { get; set; }
}