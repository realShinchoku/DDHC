namespace StudentService.DTOs;

public class StudentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ClassName { get; set; }
    public Guid ClassId { get; set; }
    public string StudentCode { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public Guid GradeId { get; set; }
    public string GradeName { get; set; }
}