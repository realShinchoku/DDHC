namespace StudentService.Params;

public class SubjectParams
{
    public string Room { get; set; }
    public DateOnly? Date { get; set; }
    public string Code { get; set; }

    public Guid? TeacherId { get; set; }
}