using System.ComponentModel.DataAnnotations;

namespace StudentService.DTOs;

public class StudentCreateDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    [MaxLength(12)] public string StudentCode { get; set; }
    public string Class { get; set; }
    public string Faculty { get; set; }
}