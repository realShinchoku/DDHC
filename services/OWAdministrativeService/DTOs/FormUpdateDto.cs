namespace OWAdministrativeService.DTOs;

public class FormUpdateDto
{
    public string Code { get; set; }
    public string Note { get; set; }
    public string Status { get; set; }
    public DateTime? DateToGetResult { get; set; }
}