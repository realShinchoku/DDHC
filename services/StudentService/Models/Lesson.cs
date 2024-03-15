using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Lessons;

namespace StudentService.Models;

public class Lesson : BaseEntity
{
    public Guid SubjectId { get; set; }
    [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime? OpenAttendanceTime { get; set; }
    public DateTime? CloseAttendanceTime { get; set; }
    public virtual ICollection<Attendance> Attendances { get; set; }
    public virtual ICollection<Wifi> Wifi { get; set; }
    public virtual ICollection<Bluetooth> Bluetooth { get; set; }
}