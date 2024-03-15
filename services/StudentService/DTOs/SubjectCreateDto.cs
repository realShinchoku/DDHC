namespace StudentService.DTOs;

public class SubjectCreateDto
{
    public string Room { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public ICollection<StudentCreateDto> Students { get; set; }
    public ICollection<LessonCreateDto> Lessons { get; set; }
}