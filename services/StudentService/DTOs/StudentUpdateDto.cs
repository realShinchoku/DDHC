namespace StudentService.DTOs;

public class StudentUpdateDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Class { get; set; }
    public string Grade { get; set; }
    public DateTime BirthDay { get; set; }
    public string Faculty { get; set; }
    public int Status { get; set; }
}