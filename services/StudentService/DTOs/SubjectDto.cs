namespace StudentService.DTOs;

public class SubjectDto
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public string Room { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsEnded { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int StudentsCount { get; set; }
    public int LessonsCount { get; set; }
    public int CurrentLessonsCount { get; set; }
}