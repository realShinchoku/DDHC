namespace OWAdministrativeService.Models;

public class StudentCardForm
{
    public string FullName { get; set; }
    public string BirthDay { get; set; }
    public string CurrentClass { get; set; }
    public string FirstClass { get; set; }
    public string StudentCode { get; set; }
    public string Course { get; set; }
    public Reason Reason { get; set; }
    public string StudentType { get; set; }
    public string Photo3X4 { get; set; }
    public string FrontIdPhoto { get; set; }
    public string BackIdPhoto { get; set; }
    public string Code { get; set; }
    public string CardReturnDate { get; set; }
    public string CreatedDate { get; set; } = DateTime.UtcNow.ToString("dd-MM-yyyy");
}

public enum Reason
{
    FirstCreate,
    PrintingError,
    Damaged,
    ReturningToSchool,
    Lost
}