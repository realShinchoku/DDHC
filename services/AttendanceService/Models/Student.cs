using MongoDB.Entities;

namespace AttendanceService.Models;

public class Student : Entity
{
    public string Name { get; set; }
    public string ClassName { get; set; }
    public string ClassId { get; set; }
    public string StudentCode { get; set; }
    public string DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string GradeId { get; set; }
    public string GradeName { get; set; }
}