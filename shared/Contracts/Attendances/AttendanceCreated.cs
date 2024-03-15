namespace Contracts.Attendances;

public class AttendanceCreated
{
    public Guid LessonId { get; set; }
    public string StudentEmail { get; set; }
    public DateTime AttendedTime { get; set; } = DateTime.UtcNow;
    public AttendanceType Type { get; set; }
}

public enum AttendanceType
{
    Presence,
    Late,
    Absent
}