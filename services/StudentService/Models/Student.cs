using Microsoft.EntityFrameworkCore;

namespace StudentService.Models;

[Index(nameof(StudentCode))]
public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Class Class { get; set; }
    public Guid ClassId { get; set; }
    public string StudentCode { get; set; }
}