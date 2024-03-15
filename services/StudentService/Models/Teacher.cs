using System.ComponentModel.DataAnnotations;

namespace StudentService.Models;

public class Teacher : BaseEntity
{
    [MaxLength(50)] public string Name { get; set; }
    [MaxLength(50)] public string Email { get; set; }
    [MaxLength(15)] public string Phone { get; set; }
    [MaxLength(50)] public string Department { get; set; }
    [MaxLength(50)] public string Faculty { get; set; }
    public virtual ICollection<Subject> Subjects { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Inactive;
}