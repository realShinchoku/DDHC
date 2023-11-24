namespace StudentService.Models;

public class Class
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    public Department Department { get; set; }
    public Guid DepartmentId { get; set; }
    public Grade Grade { get; set; }
    public Guid GradeId { get; set; }
}