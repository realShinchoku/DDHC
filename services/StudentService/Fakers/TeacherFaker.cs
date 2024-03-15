using Bogus;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class TeacherFaker : Faker<Teacher>
{
    public TeacherFaker() : base("vi")
    {
        RuleFor(x => x.Name, f => f.Person.FullName);
        RuleFor(x => x.Email, f => f.Person.Email + f.Random.String2(7));
        RuleFor(x => x.Phone, f => f.Random.Replace("############"));
        RuleFor(x => x.Department, f => f.Company.CompanyName());
        RuleFor(x => x.Faculty, f => f.Company.CompanyName());
        RuleFor(x => x.Subjects, f => new SubjectFaker().Generate(3));
    }
}