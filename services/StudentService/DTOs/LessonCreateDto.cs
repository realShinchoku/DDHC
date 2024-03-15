namespace StudentService.DTOs;

public class LessonCreateDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}