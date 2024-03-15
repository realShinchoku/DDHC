﻿namespace Contracts.Lessons;

public class LessonUpdated
{
    public Guid SubjectId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Wifi> Wifi { get; set; }
    public ICollection<Bluetooth> Bluetooth { get; set; }
    public ICollection<string> StudentEmails { get; set; }
    public DateTime? OpenAttendanceTime { get; set; }
    public DateTime? CloseAttendanceTime { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Room { get; set; }
    public bool IsEnded { get; set; }
    public string TeacherName { get; set; }
    public Guid Id { get; set; }
}