using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentService.Models;

[Index(nameof(StudentCode), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Student : BaseEntity
{
    [MaxLength(50)] public string Name { get; set; }
    [MaxLength(50)] public string Email { get; set; }
    [MaxLength(15)] public string Phone { get; set; }
    public DateOnly? BirthDay { get; set; }
    [MaxLength(12)] public string StudentCode { get; set; }
    [MaxLength(10)] public string Class { get; set; }
    [MaxLength(10)] public string Grade { get; set; }
    [MaxLength(50)] public string Faculty { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Inactive;
    public virtual ICollection<Attendance> Attendances { get; set; }
    public virtual ICollection<Subject> Subjects { get; set; }
}