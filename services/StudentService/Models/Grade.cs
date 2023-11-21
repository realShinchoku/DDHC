namespace StudentService.Models;

public class Grade
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}