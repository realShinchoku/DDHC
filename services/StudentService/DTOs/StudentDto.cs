namespace StudentService.DTOs;

public class StudentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string StudentCode { get; set; }
    public string Class { get; set; }
    public string Grade { get; set; }
    public string Faculty { get; set; }
    public int Status { get; set; }
    public DateOnly? BirthDay { get; set; }
}