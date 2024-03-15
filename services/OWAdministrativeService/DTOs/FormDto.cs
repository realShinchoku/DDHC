using OWAdministrativeService.Models;

namespace OWAdministrativeService.DTOs;

public class FormDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Email { get; set; }
    public object Body { get; set; }
    public string FileSrc { get; set; }
    public string FileName { get; set; }
    public FormType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public FormStatus Status { get; set; }
    public DateTime? DateToGetResult { get; set; }
    public string Note { get; set; }
}