namespace OWAdministrativeService.Models;

public class StudentVerifyForm
{
    public string FullName { get; set; }
    public string Sex { get; set; }
    public string BirthDay { get; set; }
    public string Class { get; set; }
    public string StudentCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Faculty { get; set; }
    public string IdNumber { get; set; }
    public string IdDateIssued { get; set; }
    public Purpose Purpose { get; set; }
    public string Code { get; set; }
}

public enum Purpose
{
    TaxReduction,
    MilitaryServicePostponement,
    PartTimeJob,
    TemporaryResidence,
    VisaApplication,
    StudyPeriodConfirmation,
    Other
}