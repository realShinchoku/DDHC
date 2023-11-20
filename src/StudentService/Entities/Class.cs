using System.ComponentModel.DataAnnotations.Schema;

namespace StudentService.Entities;

public class Class
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    public Department Department { get; set; }
    public Guid DepartmentId { get; set; }
    public Course Course { get; set; }
    public Guid CourseId { get; set; }
}