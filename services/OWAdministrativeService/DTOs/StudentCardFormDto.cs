using OWAdministrativeService.Models;

namespace OWAdministrativeService.DTOs;

public class StudentCardFormDto
{
    public string FullName { get; set; }
    public DateTime BirthDay { get; set; }
    public string CurrentClass { get; set; }
    public string FirstClass { get; set; }
    public string StudentCode { get; set; }
    public string Course { get; set; }
    public Reason Reason { get; set; }
    public string StudentType { get; set; }
    public IFormFile Photo3X4 { get; set; }
    public IFormFile FrontIdPhoto { get; set; }
    public IFormFile BackIdPhoto { get; set; }
}