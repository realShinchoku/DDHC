using Contracts.Attendances;

namespace StudentService.DTOs;

public class AttendanceStudentDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentCode { get; set; }
    public ICollection<AttendanceLessonDto> Lessons { get; set; }
}

public class AttendanceLessonDto
{
    public Guid LessonId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime AttendedTime { get; set; }
    public AttendanceType? Type { get; set; }
}