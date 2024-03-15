using StudentService.Models;

namespace StudentService.DTOs;

public class TeacherDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Department { get; set; }
    public string Faculty { get; set; }
    public UserStatus Status { get; set; }
}