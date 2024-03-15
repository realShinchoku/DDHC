using OWAdministrativeService.Models;

namespace OWAdministrativeService.DTOs;

public class StudentVerifyFormDto
{
    public string FullName { get; set; }
    public string Sex { get; set; }
    public DateTime BirthDay { get; set; }
    public string Class { get; set; }
    public string StudentCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Faculty { get; set; }
    public string IdNumber { get; set; }
    public DateTime IdDateIssued { get; set; }
    public Purpose Purpose { get; set; }
}